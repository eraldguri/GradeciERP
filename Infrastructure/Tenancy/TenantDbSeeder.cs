
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Tenancy;

public class TenantDbSeeder : ITenantDbSeeder
{
    private readonly TenantDbContext _tenantDbContext;
    private readonly IServiceProvider _serviceProvider;

    public TenantDbSeeder(TenantDbContext tenantDbContext, IServiceProvider serviceProvider)
    {
        _tenantDbContext = tenantDbContext;
        _serviceProvider = serviceProvider;
    }

    public async Task InitializeDatabaseAsync(CancellationToken ct)
    {
        await InitializeDatabaseWithTenantAsync(ct);

        foreach (var tenant in await _tenantDbContext.TenantInfo.ToListAsync(ct))
        {
            await InitializeApplicationDbForTenant(tenant, ct);
        }
    }

    private async Task InitializeDatabaseWithTenantAsync(CancellationToken ct)
    {
        if (_tenantDbContext.Database.GetMigrations().Any())
        {
            await _tenantDbContext.Database.MigrateAsync(ct);
        }

        if (await _tenantDbContext.TenantInfo.FindAsync([TenancyConstants.Root.Id], ct) is null)
        {
            var rootTenant = new CompanyTenantInfo
            {
                Id = TenancyConstants.Root.Id,
                Identifier = TenancyConstants.Root.Id,
                Name = TenancyConstants.Root.Name,
                Email = TenancyConstants.Root.Email,
                FirstName = TenancyConstants.Root.Name,
                LastName = TenancyConstants.Root.Name,
                IsActive = true,
                ValidUpTo = DateTime.UtcNow.AddYears(2)
            };

            await _tenantDbContext.AddAsync(rootTenant, ct);
            await _tenantDbContext.SaveChangesAsync(ct);
        }
    }

    private async Task InitializeApplicationDbForTenant(CompanyTenantInfo currentTenant, CancellationToken ct)
    {
        using var scope = _serviceProvider.CreateScope();

        _serviceProvider.GetRequiredService<IMultiTenantContextSetter>()
            .MultiTenantContext = new MultiTenantContext<CompanyTenantInfo>()
            {
                TenantInfo = currentTenant
            };

        await scope.ServiceProvider.GetRequiredService<ApplicationDbSeeder>()
            .InitialiazeDatabaseAsync(ct);
    }
}
