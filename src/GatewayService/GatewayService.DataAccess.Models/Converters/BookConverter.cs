using GatewayService.DataAccess.Models.Books;
using GatewayService.Domain.Models.Books;

namespace GatewayService.DataAccess.Models.Converters;

public static class BookConverter
{
    public static Book ToDomain(this BookDto bookDto)
    {
        return new Book
        {
            Author = bookDto.Author,
            Genre = bookDto.Genre,
            Condition = bookDto.Condition,
            AvailableCount = bookDto.AvailableCount,
            BookUuid = bookDto.BookUuid,
            Name = bookDto.Name
        };
    }

    public static BookPaged ToDomain(this BookPagedDto bookPagedDto)
    {
        return new BookPaged
        {
            Items = bookPagedDto.Items.ConvertAll(b => b.ToDomain()),
            Page = bookPagedDto.Page,
            PageSize = bookPagedDto.PageSize,
            TotalItems = bookPagedDto.TotalItems
        };
    }
}