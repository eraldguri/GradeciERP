namespace Application.Features.Identity.Users.Models.Requests;

public class ChangeUserStatusRequest
{
    public string? UserId { get; set; }
    public bool Activation { get; set; }
}
