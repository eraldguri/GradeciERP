using Finbuckle.MultiTenant.Abstractions;
using Infrastructure.Constants;
using Infrastructure.Identity.Models;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class ApplicationDbSeeder(
    IMultiTenantContextAccessor<OrgTenantInfo> tenantInfoContextAccessor,
         RoleManager<ApplicationRole> roleManager,
         UserManager<ApplicationUser> userManager,
         ApplicationDbContext applicationDbContext)
{
    public async Task InitialiazeDatabaseAsync(CancellationToken cancellationToken)
    {
        if (applicationDbContext.Database.GetMigrations().Any())
        {
            if ((await applicationDbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
            {
                await applicationDbContext.Database.MigrateAsync(cancellationToken);
            }

            if (await applicationDbContext.Database.CanConnectAsync(cancellationToken))
            {
                await InitializeDefaultRolesAsync(cancellationToken);
                await InitializeAdminUserAsync();
            }
        }
    }

    private async Task InitializeDefaultRolesAsync(CancellationToken ct)
    {
        foreach (var roleName in RoleConstants.DefaultRoles)
        {
            var incomingRole = await roleManager.FindByNameAsync(roleName);

            if (incomingRole is null)
            {
                incomingRole = new ApplicationRole
                {
                    Name = roleName,
                    Description = $"{roleName} Role"
                };

                var createResult = await roleManager.CreateAsync(incomingRole);
                if (!createResult.Succeeded)
                {
                    throw new InvalidOperationException($"Failed to create role '{roleName}': {string.Join("; ", createResult.Errors.Select(e => $"{e.Code}: {e.Description}"))}");
                }
            }

            if (roleName == RoleConstants.Basic)
            {
                await AssignPermissionsToRoleAsync(CompanyPermissions.Basic, incomingRole, ct);      
            }
            else if (roleName == RoleConstants.Admin)
            {
                await AssignPermissionsToRoleAsync(CompanyPermissions.Admin, incomingRole, ct);

                if (tenantInfoContextAccessor?.MultiTenantContext?.TenantInfo?.Id == TenancyConstants.Root.Id)
                {
                    await AssignPermissionsToRoleAsync(CompanyPermissions.Root, incomingRole, ct);       
                }
            }
        }
    }

    private async Task AssignPermissionsToRoleAsync(IReadOnlyList<CompanyPermission> incomingRolePermissions, 
        ApplicationRole role, CancellationToken ct)
    {
        var currentlyAssignedClaims = await roleManager.GetClaimsAsync(role);

        foreach (var incomingPermission in incomingRolePermissions)
        {
            if (!currentlyAssignedClaims.Any(c => c.Type == ClaimConstants.Permission && c.Value == incomingPermission.Name))
            {
                await applicationDbContext.RoleClaims.AddAsync(new ApplicationRoleClaims
                {
                    RoleId = role.Id,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = incomingPermission.Name,
                    Description = incomingPermission.Description,
                    Group = incomingPermission.Group
                }, ct);

                await applicationDbContext.SaveChangesAsync(ct);
            }
        }
    }

    private async Task InitializeAdminUserAsync()
    {
        if (string.IsNullOrEmpty(tenantInfoContextAccessor?.MultiTenantContext?.TenantInfo?.Email)) return;
        
        if (await userManager.Users
            .SingleOrDefaultAsync(user => user.Email == tenantInfoContextAccessor.MultiTenantContext.TenantInfo.Email) 
            is not ApplicationUser incomingUser)
        {
            incomingUser = new ApplicationUser
            {
                FirstName = tenantInfoContextAccessor.MultiTenantContext.TenantInfo.FirstName,
                LastName = tenantInfoContextAccessor.MultiTenantContext.TenantInfo.LastName,
                Email = tenantInfoContextAccessor.MultiTenantContext.TenantInfo.Email,
                UserName = tenantInfoContextAccessor.MultiTenantContext.TenantInfo.Email,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                NormalizedEmail = tenantInfoContextAccessor.MultiTenantContext.TenantInfo.Email.ToUpperInvariant(),
                NormalizedUserName = tenantInfoContextAccessor.MultiTenantContext.TenantInfo.Email.ToUpperInvariant(),
                IsActive = true
            };

            var passwordHash = new PasswordHasher<ApplicationUser>();

            incomingUser.PasswordHash = passwordHash.HashPassword(incomingUser, TenancyConstants.DefaultPassword);
            await userManager.CreateAsync(incomingUser);
        }

        if (!await userManager.IsInRoleAsync(incomingUser, RoleConstants.Admin))
        {
            await userManager.AddToRoleAsync(incomingUser, RoleConstants.Admin);
        }
    }
}
