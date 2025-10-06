using System.Text.Json.Serialization;

namespace LibrarySystem.Web.Dto;

public class LibraryPagedDto
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
    public List<LibraryDto> Items { get; set; }

    public LibraryPagedDto(int page, int pageSize, int totalItems, List<LibraryDto> items)
    {
        Page = page;
        PageSize = pageSize;
        TotalItems = totalItems;
        Items = items;
    }

    public LibraryPagedDto()
    {
    }
}