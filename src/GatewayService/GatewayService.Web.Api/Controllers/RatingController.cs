using System.Text.Json;
using GatewayService.Configuration;
using GatewayService.Web.Dto;
using GatewayService.Web.Dto.Ratings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GatewayService.Web.Api.Controllers;

[ApiController]
[Route("/api/v1/rating")]
public class RatingController : ControllerBase
{
    private readonly RatingSystemConfiguration _ratingSystemConfiguration;

    public RatingController(IOptions<RatingSystemConfiguration> ratingSystemConfiguration)
    {
        _ratingSystemConfiguration = ratingSystemConfiguration.Value;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<RatingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRatingsByUsernameAsync([FromHeader(Name = "X-User-Name")] string username)
    {
        using var client = new HttpClient();

        using var request = new HttpRequestMessage(HttpMethod.Get,
            $"{_ratingSystemConfiguration.IpAddress}/{_ratingSystemConfiguration.BaseUrl}");
        request.Headers.Add(_ratingSystemConfiguration.UsernameHeader, username);

        using var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        
        var rating = JsonSerializer.Deserialize<RatingDto>(json);
        
        return Ok(rating);
    }
}