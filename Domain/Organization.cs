namespace Domain;

public class Organization
{
    public int Id { get; init; }
    public string? Name { get; set; }
    public string? Country { get; set; }
    public string? TimeZone { get; set; }
    public string? Currency { get; set; }
    public DateTime EstablishedDate { get; set; }
}
