using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Infrastructure.Identity.Auth;
using Infrastructure.Constants;
using Application.Features.Identity.Users.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Identity.Users.Commands;
using Application.Features.Identity.Users.Queries;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers;

[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
public class UsersController : BaseApiController
{
    [HttpPost("register")]
    [ShouldHavePermission(CompanyAction.Create, CompanyFeature.Users)]
    public async Task<IActionResult> RegisterUserAsync([FromBody] CreateUserRequest createUser)
    {
        var response = await Sender.Send(new CreateUserCommand { CreateUser = createUser });

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPut("update")]
    [ShouldHavePermission(CompanyAction.Update, CompanyFeature.Users)]
    public async Task<IActionResult> UpdateUserDetailsAsync([FromBody] UpdateUserRequest updateUser)
    {
        var response = await Sender.Send(new  UpdateUserCommand { UpdateUser = updateUser });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    [HttpPut("update-status")]
    [ShouldHavePermission(CompanyAction.Update, CompanyFeature.Users)]
    public async Task<IActionResult> ChangeUserStatusAsync([FromBody] ChangeUserStatusRequest changeUserStatus)
    {
        var response = await Sender.Send(new UpdateUserStatusCommand { ChangeUserStatus = changeUserStatus });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    [HttpPut("update-roles/{userId}")]
    [ShouldHavePermission(CompanyAction.Update, CompanyFeature.UserRoles)]
    public async Task<IActionResult> UpdateUserRolesAsync([FromBody] UserRolesRequest userRoleRequest, string userId)
    {
        var response = await Sender.Send(new UpdateUserRolesCommand { UserRolesRequest = userRoleRequest, UserId = userId });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    [HttpDelete("delete/{userId}")]
    [ShouldHavePermission(CompanyAction.Delete, CompanyFeature.Users)]
    public async Task<IActionResult> DeleteUserAsync(string userId)
    {
        var response = await Sender.Send(new DeleteUserCommand { UserId = userId });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    [HttpGet("all")]
    [ShouldHavePermission(CompanyAction.Read, CompanyFeature.Users)]
    public async Task<IActionResult> GetUserAsync()
    {
        var response = await Sender.Send(new GetAllUsersQuery());
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    [HttpGet("{userId}")]
    [ShouldHavePermission(CompanyAction.Read, CompanyFeature.Users)]
    public async Task<IActionResult> GetUserByIdAsync(string userId)
    {
        var response = await Sender.Send(new GetUserByIdQuery { UserId = userId });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    [HttpGet("permissions/{userId}")]
    [ShouldHavePermission(CompanyAction.Read, CompanyFeature.RoleClaims)]
    public async Task<IActionResult> GetUserPermissionsAsync(string userId)
    {
        var response = await Sender.Send(new GetUserPermissionsQuery { UserId = userId });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    [HttpGet("user-roles/{userId}")]
    [ShouldHavePermission(CompanyAction.Read, CompanyFeature.UserRoles)]
    public async Task<IActionResult> GetUserRolesAsync(string userId)
    {
        var response = await Sender.Send(new GetUserRolesQuery { UserId = userId });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    [HttpPut("change-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ChangeUserPasswordAsync([FromBody] ChangePasswordRequest changePassword)
    {
        var response = await Sender.Send(new ChangeUserPasswordCommand { ChangePassword = changePassword });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return NotFound(response);
    }
}
