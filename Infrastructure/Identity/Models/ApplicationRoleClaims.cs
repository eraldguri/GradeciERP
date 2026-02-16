using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Models;

public class ApplicationRoleClaims : IdentityRoleClaim<string>
{
    public string? Description { get; set; }
    public string? Group { get; set; }
}
