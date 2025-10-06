using System.Text.Json.Serialization;

namespace GatewayService.Web.Dto.Books;

public class BookPagedDto
{
    [JsonRequired]
    [JsonPropertyName("page")]
    public int Page { get; set; }
    
    [JsonRequired]
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }
    
    [JsonRequired]
    [JsonPropertyName("totalElements")]
    public int TotalItems { get; set; }
    
    [JsonRequired]
    [JsonPropertyName("items")]
    public List<BookDto> Items { get; set; }

    public BookPagedDto(int page, int pageSize, int totalItems, List<BookDto> items)
    {
        Page = page;
        PageSize = pageSize;
        TotalItems = totalItems;
        Items = items;
    }

    public BookPagedDto()
    {
    }
}