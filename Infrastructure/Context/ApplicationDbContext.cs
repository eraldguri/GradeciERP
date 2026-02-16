using Domain;
using Finbuckle.MultiTenant.Abstractions;
using Infrastructure.Tenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Infrastructure.Context;

public class ApplicationDbContext : BaseDbContext
{
    public ApplicationDbContext(
        IMultiTenantContextAccessor<CompanyTenantInfo> tenantInfoContextAccessor,
        DbContextOptions<ApplicationDbContext> options) : base(tenantInfoContextAccessor, options)
    {
        
    }

    public DbSet<Company> Companies => Set<Company>();
}
