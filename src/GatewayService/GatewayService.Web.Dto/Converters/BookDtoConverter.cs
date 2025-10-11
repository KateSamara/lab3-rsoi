using GatewayService.Domain.Models.Books;
using GatewayService.Web.Dto.Books;

namespace GatewayService.Web.Dto.Converters;

public static class BookDtoConverter
{
    public static BookDto ToDto(this Book book)
    {
        return new BookDto(bookUuid: book.BookUuid,
            author: book.Author,
            name: book.Name,
            genre: book.Genre,
            condition: book.Condition,
            availableCount: book.AvailableCount);
    }

    public static BookPagedDto ToDto(this BookPaged books)
    {
        return new BookPagedDto(page: books.Page,
            pageSize: books.PageSize,
            totalItems: books.TotalItems,
            items: books.Items.ConvertAll(b => b.ToDto()));
    }
    
    public static BookShortDto ToDto(this BookShort book)
    {
        return new BookShortDto(bookUuid: book.BookUuid,
            author: book.Author,
            name: book.Name,
            genre: book.Genre);
    }
}