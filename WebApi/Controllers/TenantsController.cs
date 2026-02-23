using Application.Features.Tenancy;
using Application.Features.Tenancy.Commands;
using Application.Features.Tenancy.Queries;
using Infrastructure.Constants;
using Infrastructure.Identity.Auth;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
public class TenantsController : BaseApiController
{
    [HttpPost("add")]
    [ShouldHavePermission(CompanyAction.Create, CompanyFeature.Tenants)]
    public async Task<IActionResult> CreateTenantAsync([FromBody] CreateTenantRequest createTenantRequest, CancellationToken ct)
    {
        var response = await Sender.Send(new CreateTenantCommand { CreateTenant = createTenantRequest });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPut("{tenantId}/activate")]
    [ShouldHavePermission(CompanyAction.Update, CompanyFeature.Tenants)]
    public async Task<IActionResult> ActivateTenantAsync(string tenantId)
    {
        var response = await Sender.Send(new ActivateTenantCommand { TenantId = tenantId });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPut("{tenantId}/deactivate")]
    [ShouldHavePermission(CompanyAction.Update, CompanyFeature.Tenants)]
    public async Task<IActionResult> DeactivateTenantAsync(string tenantId)
    {
        var response = await Sender.Send(new DeactivateTenantCommand { TenantId = tenantId });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPut("upgrade")]
    [ShouldHavePermission(CompanyAction.UpgradeSubscription, CompanyFeature.Tenants)]
    public async Task<IActionResult> UpgradeSubscriptionAsync([FromBody] UpdateTenantSubscriptionRequest updateTenant)
    {
        var response = await Sender.Send(new UpdateTenantSubscriptionCommand { UpdateTenantSubscription = updateTenant });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpGet("{tenantId}")]
    [ShouldHavePermission(CompanyAction.Read, CompanyFeature.Tenants)]
    public async Task<IActionResult> GenantTenantByIdAsync(string tenantId)
    {
        var response = await Sender.Send(new GetTenantByIdQuery { TenantId = tenantId });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpGet("all")]
    [ShouldHavePermission(CompanyAction.Read, CompanyFeature.Tenants)]
    public async Task<IActionResult> GenantTenantsAsync()
    {
        var response = await Sender.Send(new GetTenantsQuery());
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}
