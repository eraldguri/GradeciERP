namespace Application.Features.Companies;

public class UpdateCompanyRequest
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Country { get; set; }
    public string? TimeZone { get; set; }
    public string? Currency { get; set; }
    public DateTime EstablishedDate { get; set; }
}
