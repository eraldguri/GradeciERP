using System.Collections.ObjectModel;

namespace Infrastructure.Constants;

public static class CompanyAction
{
    public const string Read = nameof(Read);
    public const string Create = nameof(Create);
    public const string Update = nameof(Update);
    public const string Delete = nameof(Delete);
    public const string RefreshToken = nameof(RefreshToken);
    public const string UpgradeSubscription = nameof(UpgradeSubscription);
}

public static class CompanyFeature
{
    public const string Tenants = nameof(Tenants);
    public const string Users = nameof(Users);
    public const string Roles = nameof(Roles);
    public const string UserRoles = nameof(UserRoles);
    public const string RoleClaims = nameof(RoleClaims);
    public const string Companies = nameof(Companies);
    public const string Tokens = nameof(Tokens);
}

public record CompanyPermission(string Action, string Feature, string Description, string Group, bool IsBasic = false, bool IsRoot = false)
{
    public string Name => NameFor(Action, Feature);

    public static string NameFor(string action, string feature) => $"Permission.{feature}.{action}";
}

public static class CompanyPermissions
{
    private static readonly CompanyPermission[] _allPermissions =
    [
        new CompanyPermission(CompanyAction.Create, CompanyFeature.Tenants, "Create Tenants", "Tenancy", IsRoot: true),
        new CompanyPermission(CompanyAction.Read, CompanyFeature.Tenants, "Read Tenants", "Tenancy", IsRoot: true),
        new CompanyPermission(CompanyAction.Update, CompanyFeature.Tenants, "Update Tenants", "Tenancy", IsRoot: true),
        new CompanyPermission(CompanyAction.UpgradeSubscription, CompanyFeature.Tenants, "Upgrade Tenant's Subscription", "Tenancy", IsRoot: true),

        new CompanyPermission(CompanyAction.Create, CompanyFeature.Users, "Create Users", "SystemAccess"),
        new CompanyPermission(CompanyAction.Update, CompanyFeature.Users, "Update Users", "SystemAccess"),
        new CompanyPermission(CompanyAction.Delete, CompanyFeature.Users, "Delete Users", "SystemAccess"),
        new CompanyPermission(CompanyAction.Read, CompanyFeature.Users, "Read Users", "SystemAccess"),

        new CompanyPermission(CompanyAction.Read, CompanyFeature.UserRoles, "Read User Roles", "SystemAccess"),
        new CompanyPermission(CompanyAction.Update, CompanyFeature.UserRoles, "Update User Roles", "SystemAccess"),

        new CompanyPermission(CompanyAction.Read, CompanyFeature.Roles, "Read Roles", "SystemAccess"),
        new CompanyPermission(CompanyAction.Create, CompanyFeature.Roles, "Create Roles", "SystemAccess"),
        new CompanyPermission(CompanyAction.Update, CompanyFeature.Roles, "Update Roles", "SystemAccess"),
        new CompanyPermission(CompanyAction.Delete, CompanyFeature.Roles, "Delete Roles", "SystemAccess"),

        new CompanyPermission(CompanyAction.Read, CompanyFeature.RoleClaims, "Read Role Claims/Permissions", "SystemAccess"),
        new CompanyPermission(CompanyAction.Update, CompanyFeature.RoleClaims, "Update Role Claims/Permissions", "SystemAccess"),

        new CompanyPermission(CompanyAction.Read, CompanyFeature.Companies, "Read Companies", "Organizations", IsBasic: true),
        new CompanyPermission(CompanyAction.Create, CompanyFeature.Companies, "Create Companies", "Organizations"),
        new CompanyPermission(CompanyAction.Update, CompanyFeature.Companies, "Update Companies", "Organizations"),
        new CompanyPermission(CompanyAction.Delete, CompanyFeature.Companies, "Delete Companies", "Organizations"),

        new CompanyPermission(CompanyAction.RefreshToken, CompanyFeature.Tokens, "Generate Refresh Token", "SystemAccess", IsBasic: true),
    ];

    public static IReadOnlyList<CompanyPermission> All { get; } 
        = new ReadOnlyCollection<CompanyPermission>(_allPermissions);   
    public static IReadOnlyList<CompanyPermission> Root { get; } 
        = new ReadOnlyCollection<CompanyPermission>(_allPermissions.Where(p => p.IsRoot).ToArray());
    public static IReadOnlyList<CompanyPermission> Admin { get; } 
        = new ReadOnlyCollection<CompanyPermission>(_allPermissions.Where(p => !p.IsRoot).ToArray());
    public static IReadOnlyList<CompanyPermission> Basic { get; } 
        = new ReadOnlyCollection<CompanyPermission>(_allPermissions.Where(p => p.IsBasic).ToArray());
}
