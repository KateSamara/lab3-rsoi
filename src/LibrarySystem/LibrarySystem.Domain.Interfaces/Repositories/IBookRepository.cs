using LibrarySystem.Domain.Models;
using LibrarySystem.Domain.Models.Books;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IBookRepository
{
    public Task<int> GetBooksCountAsync();
    
    public Task AddBookAsync(Book book);
    
    public Task<List<Book>> GetBooksByIdsAsync(List<Guid> ids);
}