using LibrarySystem.Domain.Models.Books;

namespace LibrarySystem.Domain.Interfaces.Services;

public interface IBookService
{
    public Task<List<Book>> GetBooksByIdsAsync(List<Guid> ids);
}