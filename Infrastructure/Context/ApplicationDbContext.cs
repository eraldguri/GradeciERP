using Domain;
using Finbuckle.MultiTenant.Abstractions;
using Infrastructure.Tenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using YamlDotNet.Core.Tokens;

namespace Infrastructure.Context;

public class ApplicationDbContext : BaseDbContext
{
    public ApplicationDbContext(
        IMultiTenantContextAccessor<CompanyTenantInfo> tenantInfoContextAccessor,
        DbContextOptions<ApplicationDbContext> options) : base(tenantInfoContextAccessor, options)
    {
        
    }

    public DbSet<Company> Companies => Set<Company>();
    public DbSet<CompanyBranch> Branches => Set<CompanyBranch>();
    public DbSet<BranchOffers> BranchOffers => Set<BranchOffers>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Company configuration
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name);
            entity.Property(c => c.Country);
            entity.Property(c => c.Currency);
            entity.Property(c => c.TimeZone);
            entity.HasMany(c => c.Branches)
                .WithOne(b => b.Company)
                .HasForeignKey(b => b.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // CompanyBranch configuration
        modelBuilder.Entity<CompanyBranch>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.BranchName);
            entity.Property(b => b.ContactEmail);
            entity.Property(b => b.ContactNumber);
            entity.Property(b => b.AddressLine1);
            entity.Property(b => b.Country);
            entity.Property(b => b.State);
            entity.Property(b => b.City);
            entity.Property(b => b.PostalCode);
            entity.Property(b => b.Description);
            entity.Property(b => b.SupportedPaymentMethod)
                .HasConversion<int>();

            // Optional: Create indexes for frequently queried fields
            entity.HasIndex(b => b.CompanyId);
            entity.HasIndex(b => b.Country);
            entity.HasIndex(b => b.City);

            // One-to-Many: Branch → Offers
            entity.HasMany(b => b.Services)
                .WithOne(o => o.CompanyBranch)
                .HasForeignKey(o => o.CompanyBranchId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // BranchOffers configuration
        modelBuilder.Entity<BranchOffers>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Name);

            // Create index for foreign key
            entity.HasIndex(o => o.CompanyBranchId);
        });
    }
}
