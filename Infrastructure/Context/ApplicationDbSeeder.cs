using Finbuckle.MultiTenant.Abstractions;
using Infrastructure.Constants;
using Infrastructure.Identity.Models;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class ApplicationDbSeeder(
    IMultiTenantContextAccessor<CompanyTenantInfo> tenantInfoContextAccessor,
         RoleManager<ApplicationRole> roleManager,
         UserManager<ApplicationUser> userManager,
         ApplicationDbContext applicationDbContext)
{
    private readonly IMultiTenantContextAccessor<CompanyTenantInfo> _tenantInfoContextAccessor = tenantInfoContextAccessor;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    public async Task InitialiazeDatabaseAsync(CancellationToken cancellationToken)
    {
        if (_applicationDbContext.Database.GetMigrations().Any())
        {
            if ((await _applicationDbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
            {
                await _applicationDbContext.Database.MigrateAsync(cancellationToken);
            }

            if (await _applicationDbContext.Database.CanConnectAsync(cancellationToken))
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
            if (await _roleManager.Roles.SingleOrDefaultAsync(role => role.Name == roleName, ct) is not ApplicationRole incomingRole)
            {
                incomingRole = new ApplicationRole
                {
                    Name = roleName,
                    Description = $"{roleName} Role"
                };

                await _roleManager.CreateAsync(incomingRole);
            }

            if (roleName == RoleConstants.Basic)
            {
                await AssignPermissionsToRoleAsync(CompanyPermissions.Basic, incomingRole, ct);      
            }
            else if (roleName == RoleConstants.Admin)
            {
                await AssignPermissionsToRoleAsync(CompanyPermissions.Admin, incomingRole, ct);

                if (_tenantInfoContextAccessor?.MultiTenantContext?.TenantInfo?.Id == TenancyConstants.Root.Id)
                {
                    await AssignPermissionsToRoleAsync(CompanyPermissions.Root, incomingRole, ct);       
                }
            }
        }
    }

    private async Task AssignPermissionsToRoleAsync(IReadOnlyList<CompanyPermission> incomingRolePermissions, 
        ApplicationRole role, CancellationToken ct)
    {
        var currentlyAssignedClaims = await _roleManager.GetClaimsAsync(role);

        foreach (var incomingPermission in incomingRolePermissions)
        {
            if (!currentlyAssignedClaims.Any(c => c.Type == ClaimConstants.Permission && c.Value == incomingPermission.Name))
            {
                await _applicationDbContext.RoleClaims.AddAsync(new ApplicationRoleClaims
                {
                    RoleId = role.Id,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = incomingPermission.Name,
                    Description = incomingPermission.Description,
                    Group = incomingPermission.Group
                }, ct);

                await _applicationDbContext.SaveChangesAsync(ct);
            }
        }
    }

    private async Task InitializeAdminUserAsync()
    {
        if (string.IsNullOrEmpty(_tenantInfoContextAccessor?.MultiTenantContext?.TenantInfo?.Email)) return;
        
        if (await _userManager.Users
            .SingleOrDefaultAsync(user => user.Email == _tenantInfoContextAccessor.MultiTenantContext.TenantInfo.Email) 
            is not ApplicationUser incomingUser)
        {
            incomingUser = new ApplicationUser
            {
                FirstName = _tenantInfoContextAccessor.MultiTenantContext.TenantInfo.FirstName,
                LastName = _tenantInfoContextAccessor.MultiTenantContext.TenantInfo.LastName,
                Email = _tenantInfoContextAccessor.MultiTenantContext.TenantInfo.Email,
                UserName = _tenantInfoContextAccessor.MultiTenantContext.TenantInfo.Email,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                NormalizedEmail = _tenantInfoContextAccessor.MultiTenantContext.TenantInfo.Email.ToUpperInvariant(),
                NormalizedUserName = _tenantInfoContextAccessor.MultiTenantContext.TenantInfo.Email.ToUpperInvariant(),
                IsActive = true
            };

            var passwordHash = new PasswordHasher<ApplicationUser>();

            incomingUser.PasswordHash = passwordHash.HashPassword(incomingUser, TenancyConstants.DefaultPassword);
            await _userManager.CreateAsync(incomingUser);
        }

        if (!await _userManager.IsInRoleAsync(incomingUser, RoleConstants.Admin))
        {
            await _userManager.AddToRoleAsync(incomingUser, RoleConstants.Admin);
        }
    }
}
