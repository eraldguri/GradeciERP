using Application.Features.Identity.Roles;
using Application.Features.Identity.Roles.Commands;
using Application.Features.Identity.Roles.Queries;
using Infrastructure.Constants;
using Infrastructure.Identity.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
public class RolesController : BaseApiController
{
    [HttpPost("add")]
    [ShouldHavePermission(CompanyAction.Create, CompanyFeature.Roles)]
    public async Task<IActionResult> AddRoleAsync([FromBody] CreateRoleRequest createRole)
    {
        var response = await Sender.Send(new CreateRoleCommand { CreateRole = createRole });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPut("update")]
    [ShouldHavePermission(CompanyAction.Update, CompanyFeature.Roles)]
    public async Task<IActionResult> UpdateRoleAsync([FromBody] UpdateRoleRequest updateRole)
    {
        var response = await Sender.Send(new UpdateRoleCommand { UpdateRole = updateRole });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPut("update-permissions")]
    [ShouldHavePermission(CompanyAction.Update, CompanyFeature.RoleClaims)]
    public async Task<IActionResult> UpdateRoleClaimsAsync([FromBody] UpdateRolePermissionsRequest updateRoleClaims)
    {
        var response = await Sender.Send(new UpdateRolePermissionsCommand { UpdateRolePermissions = updateRoleClaims });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpDelete("delete/{roleId}")]
    [ShouldHavePermission(CompanyAction.Delete, CompanyFeature.Roles)]
    public async Task<IActionResult> DeleteRoleAsync(string roleId)
    {
        var response = await Sender.Send(new DeleteRoleCommand { RoleId = roleId });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpGet("all")]
    [ShouldHavePermission(CompanyAction.Read, CompanyFeature.Roles)]
    public async Task<IActionResult> GetRolesAsync()
    {
        var response = await Sender.Send(new GetRolesQuery());
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpGet("partial/{roleId}")]
    [ShouldHavePermission(CompanyAction.Read, CompanyFeature.Roles)]
    public async Task<IActionResult> GetPartialRoleByIdAsync(string roleId)
    {
        var response = await Sender.Send(new GetRoleByIdQuery { RoleId = roleId });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpGet("full/{roleId}")]
    [ShouldHavePermission(CompanyAction.Read, CompanyFeature.Roles)]
    public async Task<IActionResult> GetDetailedRoleByIdAsync(string roleId)
    {
        var response = await Sender.Send(new GetRoleWithPermissionsQuery { RoleId = roleId });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}
