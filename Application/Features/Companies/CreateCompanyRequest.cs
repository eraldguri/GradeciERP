namespace Application.Features.Companies;

public class CreateCompanyRequest
{
    public string? Name { get; set; }
    public string? Country { get; set; }
    public string? TimeZone { get; set; }
    public string? Currency { get; set; }
    public DateTime EstablishedDate { get; set; }
}
