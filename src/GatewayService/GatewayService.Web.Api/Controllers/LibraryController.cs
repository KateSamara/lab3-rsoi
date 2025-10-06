using System.Text.Json;
using GatewayService.Configuration;
using GatewayService.Web.Dto;
using GatewayService.Web.Dto.Books;
using GatewayService.Web.Dto.Libraries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GatewayService.Web.Api.Controllers;

[ApiController]
[Route("/api/v1/libraries")]
public class LibraryController : ControllerBase
{
    private readonly LibrarySystemConfiguration _librarySystemConfiguration;

    public LibraryController(IOptions<LibrarySystemConfiguration> librarySystemConfiguration)
    {
        _librarySystemConfiguration = librarySystemConfiguration.Value;
    }

    [HttpGet]
    [ProducesResponseType(typeof(LibraryPagedDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetLibrariesPagedAsync([FromQuery] int page,
        [FromQuery] int size,
        [FromQuery] string city)
    {
        using var client = new HttpClient();
        
        using var request = new HttpRequestMessage(HttpMethod.Get,
            $"{_librarySystemConfiguration.IpAddress}/{_librarySystemConfiguration.BaseUrl}" +
            $"?page={page}&size={size}&city={city}");
        
        using var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        
        var libraryPaged = JsonSerializer.Deserialize<LibraryPagedDto>(json);
        return Ok(libraryPaged);
    }
    
    [HttpGet("{libraryUid}/books")]
    [ProducesResponseType(typeof(BookPagedDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBooksPagedByLibraryUuid([FromRoute] Guid libraryUid,
        [FromQuery] int page,
        [FromQuery] int size,
        [FromQuery] bool showAll)
    {
        using var client = new HttpClient();
        
        using var request = new HttpRequestMessage(HttpMethod.Get,
            $"{_librarySystemConfiguration.IpAddress}/{_librarySystemConfiguration.BaseUrl}/" +
            $"{libraryUid}/{_librarySystemConfiguration.GetBooksSuffix}?page={page}&size={size}&showAll={showAll}");
        
        using var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        
        var booksPaged = JsonSerializer.Deserialize<BookPagedDto>(json);
        
        return Ok(booksPaged);
    }
}