using Application.Features.Companies;
using Application.Features.Companies.Commands;
using Application.Features.Companies.Queries;
using Infrastructure.Constants;
using Infrastructure.Identity.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
public class CompaniesController : BaseApiController
{
    [HttpPost("add")]
    [ShouldHavePermission(CompanyAction.Create, CompanyFeature.Companies)]
    public async Task<IActionResult> CreateCompanyAsync([FromBody] CreateCompanyRequest createCompany)
    {
        var response = await Sender.Send(new CreateCompanyCommand { CreateCompany = createCompany });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPut("update")]
    [ShouldHavePermission(CompanyAction.Update, CompanyFeature.Companies)]
    public async Task<IActionResult> UpdateCompanyAsync([FromBody] UpdateCompanyRequest updateCompany)
    {
        var response = await Sender.Send(new UpdateCompanyCommand { UpdateCompany = updateCompany });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    [HttpDelete("delete/{id}")]
    [ShouldHavePermission(CompanyAction.Delete, CompanyFeature.Companies)]
    public async Task<IActionResult> DeleteCompanyAsync(int companyId)
    {
        var response = await Sender.Send(new DeleteCompanyCommand { CompanyId = companyId });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    [HttpGet("by-id/{companyId}")]
    [ShouldHavePermission(CompanyAction.Read, CompanyFeature.Companies)]
    public async Task<IActionResult> GetCompanyByIdAsync(int companyId)
    {
        var response = await Sender.Send(new GetCompanyByIdQuery { CompanyId = companyId });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    [HttpGet("by-name/{name}")]
    [ShouldHavePermission(CompanyAction.Read, CompanyFeature.Companies)]
    public async Task<IActionResult> GetCompanyByNameAsync(string name)
    {
        var response = await Sender.Send(new GetCompanyByNameQuery { Name = name });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    [HttpGet("all")]
    [ShouldHavePermission(CompanyAction.Read, CompanyFeature.Companies)]
    public async Task<IActionResult> GetAllCompanies()
    {
        var response = await Sender.Send(new GetCompaniesQuery());
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return NotFound(response);
    }
}
