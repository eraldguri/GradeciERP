namespace Domain;

public class BranchOffers
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? CompanyBranchId { get; set; }
    public CompanyBranch? CompanyBranch { get; set; }
}
