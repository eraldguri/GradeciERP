using Application.Features.Tenancy;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Infrastructure.Context;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Tenancy;

public class TenantService(IMultiTenantStore<OrgTenantInfo> tenantsStore, IServiceProvider serviceProvider)
    : ITenantService
{
    public async Task<string> ActivateAsync(string id)
    {
        var tenantInDb = await tenantsStore.TryGetAsync(id);
        tenantInDb!.IsActive = true;

        await tenantsStore.TryUpdateAsync(tenantInDb);
        return tenantInDb.Identifier!;
    }

    public async Task<string> CreateTenantAsync(CreateTenantRequest createTenant, CancellationToken ct)
    {
        var newTenant = new OrgTenantInfo
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

        await tenantsStore.TryAddAsync(newTenant);

        // Seed the tenant's database
        using var scope = serviceProvider.CreateScope();

        serviceProvider.GetRequiredService<IMultiTenantContextSetter>()
            .MultiTenantContext = new MultiTenantContext<OrgTenantInfo>
            {
                TenantInfo = newTenant
            };
        await scope.ServiceProvider.GetRequiredService<ApplicationDbSeeder>()
            .InitialiazeDatabaseAsync(ct);

        return newTenant.Identifier!;
    }

    public async Task<string> DeactivateAsync(string id)
    {
        var testInDb = await tenantsStore.TryGetAsync(id);
        testInDb!.IsActive = false;

        await tenantsStore.TryUpdateAsync(testInDb);
        return testInDb.Identifier!;
    }

    public async Task<TenantResponse> GetTenantByIdAsync(string id)
    {
        var tenantInDb = await tenantsStore.TryGetAsync(id);

        return tenantInDb.Adapt<TenantResponse>();
    }

    public async Task<List<TenantResponse>> GetTenantsAsync()
    {
        var tenantsInDb = await tenantsStore.GetAllAsync();
        return tenantsInDb.Adapt<List<TenantResponse>>();
    }

    public async Task<string> UpdateSubscriptionAsync(UpdateTenantSubscriptionRequest updateTenantSubscription)
    {
        var tenantInDb = await tenantsStore.TryGetAsync(updateTenantSubscription.TenantId!);
        tenantInDb!.ValidUpTo = updateTenantSubscription.NewExpiryDate;

        await tenantsStore.TryUpdateAsync(tenantInDb);

        return tenantInDb.Identifier!;
    }
}
