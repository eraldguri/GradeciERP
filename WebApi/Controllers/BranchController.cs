using Application.Features.Companies.Branch;
using Infrastructure.Constants;
using Infrastructure.Identity.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class BranchController : BaseApiController
    {
        [HttpPost("add")]
        [ShouldHavePermission(CompanyAction.Create, CompanyFeature.Companies)]
        public async Task<IActionResult> CreateBranchAsync([FromBody] CreateBranchRequest request)
        {
            var response = await Sender.Send(new CreateBranchCommand { CreateBranchRequest = request });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
