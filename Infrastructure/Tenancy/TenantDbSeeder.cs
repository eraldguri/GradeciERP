
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Tenancy;

public class TenantDbSeeder(TenantDbContext tenantDbContext, IServiceProvider serviceProvider)
    : ITenantDbSeeder
{
    public async Task InitializeDatabaseAsync(CancellationToken ct)
    {
        await InitializeDatabaseWithTenantAsync(ct);

        foreach (var tenant in await tenantDbContext.TenantInfo.ToListAsync(ct))
        {
            await InitializeApplicationDbForTenant(tenant, ct);
        }
    }

    private async Task InitializeDatabaseWithTenantAsync(CancellationToken ct)
    {
        if (tenantDbContext.Database.GetMigrations().Any())
        {
            await tenantDbContext.Database.MigrateAsync(ct);
        }

        if (await tenantDbContext.TenantInfo.FindAsync([TenancyConstants.Root.Id], ct) is null)
        {
            var rootTenant = new OrgTenantInfo
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

            await tenantDbContext.AddAsync(rootTenant, ct);
            await tenantDbContext.SaveChangesAsync(ct);
        }
    }

    private async Task InitializeApplicationDbForTenant(OrgTenantInfo currentTenant, CancellationToken ct)
    {
        using var scope = serviceProvider.CreateScope();

        serviceProvider.GetRequiredService<IMultiTenantContextSetter>()
            .MultiTenantContext = new MultiTenantContext<OrgTenantInfo>()
            {
                TenantInfo = currentTenant
            };

        await scope.ServiceProvider.GetRequiredService<ApplicationDbSeeder>()
            .InitialiazeDatabaseAsync(ct);
    }
}
