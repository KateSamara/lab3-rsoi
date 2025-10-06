using LibrarySystem.Domain.Interfaces.Services;
using LibrarySystem.Web.Dto;
using LibrarySystem.Web.Dto.Converters;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Web.Api.Controllers;

[ApiController]
[Route("/api/v1/books")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
    }
    
    [HttpGet("ids")]
    [ProducesResponseType(typeof(List<BookDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetLibrariesByIdsAsync([FromQuery] List<Guid> ids)
    {
        var books = await _bookService.GetBooksByIdsAsync(ids);
        
        return Ok(books.ConvertAll(b => b.ToDto()));
    }
}