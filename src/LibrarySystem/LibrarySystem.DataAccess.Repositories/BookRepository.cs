using LibrarySystem.DataAccess.Context;
using LibrarySystem.DataAccess.Models.Converters;
using LibrarySystem.Domain.Exceptions.Repositories;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Domain.Models;
using LibrarySystem.Domain.Models.Books;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.DataAccess.Repositories;

public class BookRepository(LibrarySystemContext context) : IBookRepository 
{
    private readonly LibrarySystemContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<int> GetBooksCountAsync()
    {
        try
        {
            return await _context.Books
                .CountAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new BookRepositoryException("There was an error getting the books count", e);
        }
    }

    public async Task AddBookAsync(Book book)
    {
        try
        {
            var bookDb = book.ToDb();
            
            _context.Books.Add(bookDb);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new BookRepositoryException("There was an error adding the book", e);
        }
    }

    public async Task<List<Book>> GetBooksByIdsAsync(List<Guid> ids)
    {
        try
        {
            var booksDb = await _context.Books
                .AsNoTracking()
                .Where(b => ids.Contains(b.BookUuid))
                .ToListAsync();

            return booksDb.ConvertAll(b => b.ToDomain());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new BookRepositoryException($"There was an error getting the books by ids = {ids}", e);
        }
    }
}