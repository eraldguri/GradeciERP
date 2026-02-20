namespace Application.Features.Identity.Tokens;

public class RefreshTokenRequest
{
    public string CurrentJwt { get; set; } = string.Empty;
    public string? CurrentRefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiryDate { get; set; }
}
