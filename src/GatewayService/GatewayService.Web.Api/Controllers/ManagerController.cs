using Microsoft.AspNetCore.Mvc;

namespace GatewayService.Web.Api.Controllers;

[ApiController]
[Route("/manage/health")]
public class ManagerController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CheckHealth()
    {
        return Ok();
    }
}