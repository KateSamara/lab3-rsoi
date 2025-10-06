using LibrarySystem.Domain.Models.Books;

namespace LibrarySystem.Web.Dto.Converters;

public static class BookDtoConverter
{
    public static BookDto ToDto(this Book book)
    {
        return new BookDto(bookUuid: book.BookUuid,
            name: book.Name,
            author: book.Author,
            genre: book.Genre,
            condition: book.Condition.ToString(),
            availableCount: book.AvailableCount);
    }
    
    public static BookPagedDto ToDto(this BookPaged bookPaged)
    {
        return new BookPagedDto(page: bookPaged.Page, 
            pageSize: bookPaged.PageSize,
            totalItems: bookPaged.TotalElements,
            items: bookPaged.Items.ConvertAll(l => l.ToDto()));
    }
}