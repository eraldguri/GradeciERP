namespace Application.Features.Companies.Branch;

public class CreateBranchRequest
{
    public int CompanyId { get; set; }
    public string? BranchName { get; set; }
    public string? ImageUrl { get; set; }
    public string? ContactNumber { get; set; }
    public string? ContactEmail { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? Country { get; set; }
    public string? State { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? Description { get; set; }
    public bool Status { get; set; }
    public DateTime? EstablishedDate { get; set; }
}
