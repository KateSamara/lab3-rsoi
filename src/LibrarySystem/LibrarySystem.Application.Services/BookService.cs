using LibrarySystem.Domain.Exceptions.Services;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Domain.Interfaces.Services;
using LibrarySystem.Domain.Models.Books;

namespace LibrarySystem.Application.Services;

public class BookService(IBookRepository bookRepository) : IBookService
{
    private readonly IBookRepository _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));

    public async Task<List<Book>> GetBooksByIdsAsync(List<Guid> ids)
    {
        try
        {
            return await _bookRepository.GetBooksByIdsAsync(ids);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new BookServiceException($"There was an error getting the books by ids = {ids}", e);
        }
    }
}