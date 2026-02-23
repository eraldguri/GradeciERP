using Application.Features.Tenancy;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Infrastructure.Context;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Tenancy;

public class TenantService : ITenantService
{
    private readonly IMultiTenantStore<CompanyTenantInfo> _tenantsStore;
    private readonly ApplicationDbSeeder _dbSeeder;
    private readonly IServiceProvider _serviceProvider;

    public TenantService(IMultiTenantStore<CompanyTenantInfo> tenantsStore, ApplicationDbSeeder dbSeeder, IServiceProvider serviceProvider)
    {
        _tenantsStore = tenantsStore;
        _dbSeeder = dbSeeder;
        _serviceProvider = serviceProvider;
    }

    public async Task<string> ActivateAsync(string id)
    {
        var tenantInDb = await _tenantsStore.TryGetAsync(id);
        tenantInDb!.IsActive = true;

        await _tenantsStore.TryUpdateAsync(tenantInDb);
        return tenantInDb.Identifier!;
    }

    public async Task<string> CreateTenantAsync(CreateTenantRequest createTenant, CancellationToken ct)
    {
        var newTenant = new CompanyTenantInfo
        {
            Id = createTenant.Identifier,
            Identifier = createTenant.Identifier,
            Name = createTenant.Name,
            ConnectionString = createTenant.ConnectionString,
            Email = createTenant.Email,
            FirstName = createTenant.FirstName,
            LastName = createTenant.LastName,
            ValidUpTo = createTenant.ValidUpTo,
            IsActive = createTenant.IsActive,
        };

        await _tenantsStore.TryAddAsync(newTenant);

        // Seed the tenant's database
        using var scope = _serviceProvider.CreateScope();

        _serviceProvider.GetRequiredService<IMultiTenantContextSetter>()
            .MultiTenantContext = new MultiTenantContext<CompanyTenantInfo>()
            {
                TenantInfo = newTenant
            };
        await scope.ServiceProvider.GetRequiredService<ApplicationDbSeeder>()
            .InitialiazeDatabaseAsync(ct);

        return newTenant.Identifier!;
    }

    public async Task<string> DeactivateAsync(string id)
    {
        var tenatstInDb = await _tenantsStore.TryGetAsync(id);
        tenatstInDb!.IsActive = false;

        await _tenantsStore.TryUpdateAsync(tenatstInDb);
        return tenatstInDb.Identifier!;
    }

    public async Task<TenantResponse> GetTenantByIdAsync(string id)
    {
        var tenantInDb = await _tenantsStore.TryGetAsync(id);

        return tenantInDb.Adapt<TenantResponse>();
    }

    public async Task<List<TenantResponse>> GetTenantsAsync()
    {
        var tenantsInDb = await _tenantsStore.GetAllAsync();
        return tenantsInDb.Adapt<List<TenantResponse>>();
    }

    public async Task<string> UpdateSubscriptionAsync(UpdateTenantSubscriptionRequest updateTenantSubscription)
    {
        var tenantInDb = await _tenantsStore.TryGetAsync(updateTenantSubscription.TenantId!);
        tenantInDb!.ValidUpTo = updateTenantSubscription.NewExpiryDate;

        await _tenantsStore.TryUpdateAsync(tenantInDb);

        return tenantInDb.Identifier!;
    }
}
