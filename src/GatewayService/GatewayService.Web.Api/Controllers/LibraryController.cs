using System.Text.Json;
using GatewayService.DataAccess.Gateways.Configuration;
using GatewayService.Domain.Interfaces.Services;
using GatewayService.Web.Dto;
using GatewayService.Web.Dto.Books;
using GatewayService.Web.Dto.Converters;
using GatewayService.Web.Dto.Libraries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GatewayService.Web.Api.Controllers;

[ApiController]
[Route("/api/v1/libraries")]
public class LibraryController : ControllerBase
{
    private readonly LibrarySystemConfiguration _librarySystemConfiguration;
    private readonly ILibraryService _libraryService;

    public LibraryController(IOptions<LibrarySystemConfiguration> librarySystemConfiguration,
        ILibraryService libraryService)
    {
        _librarySystemConfiguration = librarySystemConfiguration.Value;
        _libraryService = libraryService ?? throw new ArgumentNullException(nameof(libraryService));
    }

    [HttpGet]
    [ProducesResponseType(typeof(LibraryPagedDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetLibrariesPagedAsync([FromQuery] int page,
        [FromQuery] int size,
        [FromQuery] string city)
    {
        var libraries = await _libraryService.GetLibrariesByCityPagedAsync(page, size, city);
        
        return Ok(libraries.ToDto());
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