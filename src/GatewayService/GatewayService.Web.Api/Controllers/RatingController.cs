using GatewayService.Domain.Interfaces.Services;
using GatewayService.Web.Dto.Converters;
using GatewayService.Web.Dto.Ratings;
using Microsoft.AspNetCore.Mvc;

namespace GatewayService.Web.Api.Controllers;

[ApiController]
[Route("/api/v1/rating")]
public class RatingController : ControllerBase
{
    private readonly IRatingService _ratingService;
    
    public RatingController(IRatingService ratingService)
    {
        _ratingService = ratingService ?? throw new ArgumentNullException(nameof(ratingService));
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<RatingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRatingsByUsernameAsync([FromHeader(Name = "X-User-Name")] string username)
    {
        var rating = await _ratingService.GetRatingsByUsernameAsync(username);
        
        return Ok(rating.ToDto());
    }
}