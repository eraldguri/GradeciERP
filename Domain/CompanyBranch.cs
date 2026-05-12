namespace Domain;

public class CompanyBranch
{
    public int Id {  get; set; }
    public string? BranchName { get; set; }
    public string? ImageUrl { get; set; }
    public ICollection<BranchOffers> Services { get; set; } = [];
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
    public SupportedPaymentMethod SupportedPaymentMethod { get; set; } = SupportedPaymentMethod.Cash;
    public DateTime? EstablishedDate { get; set; }

    public int? CompanyId { get; set; }
    public Company? Company { get; set; }
}
