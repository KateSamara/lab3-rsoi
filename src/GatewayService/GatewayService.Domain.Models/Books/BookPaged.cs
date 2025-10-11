namespace GatewayService.Domain.Models.Books;

public record BookPaged
{
    public required int Page { get; init; }
    
    public required int PageSize { get; init; }
    
    public required int TotalItems { get; init; }
    
    public required List<Book> Items { get; init; }
}