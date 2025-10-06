using Microsoft.AspNetCore.Mvc;
using RatingSystem.Domain.Interfaces.Services;
using RatingSystem.Web.Dto;
using RatingSystem.Web.Dto.Converters;

namespace RatingSystem.Web.Api.Controllers;

[ApiController]
[Route("/api/v1/ratings")]
public class RatingController : ControllerBase
{
    private readonly IRatingService _ratingService;

    public RatingController(IRatingService ratingService)
    {
        _ratingService = ratingService ?? throw new ArgumentNullException(nameof(ratingService));
    }

    [HttpGet]
    [ProducesResponseType(typeof(RatingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRatingByUsernameAsync([FromHeader(Name = "X-User-Name")] string username)
    {
        var rating = await _ratingService.GetRatingByUsernameAsync(username);
        
        return Ok(rating.ToDto());
    }

    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateRatingAsync([FromHeader(Name = "X-User-Name")] string username,
        [FromQuery] int starDifference)
    {
        await _ratingService.UpdateRatingAsync(username, starDifference);
        
        return Ok();
    }
}