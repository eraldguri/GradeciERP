namespace Application;

public class JwtSettings
{
    public string Secret { get; set; } = string.Empty;
    public int TokenExpiryTimeInMinutes { get; set; }
    public int RefreshTokenExpiryTimeInDays { get; set; }
}
