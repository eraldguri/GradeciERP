using Domain;
using Finbuckle.MultiTenant.Abstractions;
using Infrastructure.Tenancy;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class ApplicationDbContext(
    IMultiTenantContextAccessor<OrgTenantInfo> tenantInfoContextAccessor,
    DbContextOptions<ApplicationDbContext> options)
    : BaseDbContext(tenantInfoContextAccessor, options)
{
    public DbSet<Organization> Organizations => Set<Organization>();
}
