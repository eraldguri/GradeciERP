using Domain;
using Finbuckle.MultiTenant;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Context;

internal static class DbConfigurations
{
    internal class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder
                .ToTable("Users", "Identity")
                .IsMultiTenant();
        }
    }

    internal class ApplicationRoleConfig : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder
                .ToTable("Roles", "Identity")
                .IsMultiTenant();

            // builder
            //     .HasIndex(role => role.NormalizedName)
            //     .IsUnique(false)
            //     .HasDatabaseName("RoleNameLegacyIndex")
            //     .HasFilter("[NormalizedName] IS NOT NULL");
            //
            // builder
            //     .HasIndex("NormalizedName", "TenantId")
            //     .IsUnique()
            //     .HasDatabaseName("RoleNameIndex")
            //     .HasFilter("[NormalizedName] IS NOT NULL");
        }
    }
    internal class ApplicationRoleClaimConfig : IEntityTypeConfiguration<ApplicationRoleClaims>
    {
        public void Configure(EntityTypeBuilder<ApplicationRoleClaims> builder) =>
            builder
                .ToTable("RoleClaims", "Identity")
                .IsMultiTenant();
    }

    internal class IdentityUserRoleConfig : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder) =>
            builder
                .ToTable("UserRoles", "Identity")
                .IsMultiTenant();
    }

    internal class IdentityUserClaimConfig : IEntityTypeConfiguration<IdentityUserClaim<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder) =>
            builder
                .ToTable("UserClaims", "Identity")
                .IsMultiTenant();
    }

    internal class IdentityUserLoginConfig : IEntityTypeConfiguration<IdentityUserLogin<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder) =>
            builder
                .ToTable("UserLogins", "Identity")
                .IsMultiTenant();
    }

    internal class IdentityUserTokenConfig : IEntityTypeConfiguration<IdentityUserToken<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder) =>
            builder
                .ToTable("UserTokens", "Identity")
                .IsMultiTenant();
    }

    internal class SchoolConfig : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder
                .ToTable("Organizations", "Businesses")
                .IsMultiTenant();

            builder
                .Property(company => company.Name)
                .IsRequired()
                .HasMaxLength(60);
        }
    }
}
