using Finbuckle.MultiTenant.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Tenancy;

public class OrgTenantInfo : ITenantInfo
{
    [MaxLength(100)]
    public string? Id { get; set; }
    
    [MaxLength(100)]
    public string? Identifier { get; set; }
    
    [MaxLength(200)]
    public string? Name { get; set; }
    
    [MaxLength(2000)] 
    public string? ConnectionString { get; init; }
    
    [MaxLength(256)]
    [EmailAddress]
    public string? Email { get; init; }
    
    [MaxLength(100)]
    public string? FirstName { get; init; }
    
    [MaxLength(100)]
    public string? LastName { get; init; }
    
    public DateTime ValidUpTo { get; set; }
    public bool IsActive { get; set; }
}