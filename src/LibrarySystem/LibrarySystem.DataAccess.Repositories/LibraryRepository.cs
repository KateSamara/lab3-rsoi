using LibrarySystem.DataAccess.Context;
using LibrarySystem.DataAccess.Models.Converters;
using LibrarySystem.Domain.Exceptions.Repositories;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Domain.Models;
using LibrarySystem.Domain.Models.Books;
using LibrarySystem.Domain.Models.Libraries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LibrarySystem.DataAccess.Repositories;

public class LibraryRepository(LibrarySystemContext context) : ILibraryRepository
{
    private readonly LibrarySystemContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<int> GetLibrariesCountAsync()
    {
        try
        {
            return await _context.Libraries
                .CountAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new LibraryRepositoryException("There was an error getting the libraries.", e);
        }
    }

    public async Task AddLibraryAsync(Library library)
    {
        try
        {
            var libraryDb = library.ToDb();
            
            _context.Libraries.Add(libraryDb);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new LibraryRepositoryException("There was an error adding the library.", e);
        }
    }

    public async Task<LibraryPaged> GetLibrariesPagedAsync(LibraryRequest libraryRequest)
    {
        try
        {
            var librariesQueryable = _context.Libraries
                .AsNoTracking()
                .Where(l => l.City == libraryRequest.City);
            var totalElements = await librariesQueryable.CountAsync();
            
            var librariesDb = await librariesQueryable
                .Skip((libraryRequest.Page - 1) * libraryRequest.Size)
                .Take(libraryRequest.Size)
                .ToListAsync();

            return new LibraryPaged
            {
                Page = libraryRequest.Page,
                PageSize = libraryRequest.Size,
                TotalItems = totalElements,
                Items = librariesDb.ConvertAll(l => l.ToDomain())
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new LibraryRepositoryException("There was an error getting the libraries.", e);
        }
    }

    public async Task<BookPaged> GetBookPagedByLibraryUuidAsync(BookRequest bookRequest)
    {
        try
        {
            var allBooksInLibraryQueryable = _context.Libraries
                .AsNoTracking()
                .Where(l => l.LibraryUuid == bookRequest.LibraryUuid)
                .Include(l => l.Books)
                .ThenInclude(lb => lb.Book)
                .SelectMany(l => l.Books);

            if (!bookRequest.ShowAll)
                allBooksInLibraryQueryable = allBooksInLibraryQueryable
                    .Where(lb => lb.AvailableCount > 0);
            
            var totalElements = await allBooksInLibraryQueryable.CountAsync();
            
            var booksDb = await allBooksInLibraryQueryable
                .Skip((bookRequest.Page - 1) * bookRequest.Size)
                .Take(bookRequest.Size)
                .ToListAsync();

            return new BookPaged
            {
                Page = bookRequest.Page,
                PageSize = bookRequest.Size,
                TotalElements = totalElements,
                Items = booksDb.ConvertAll(lb => lb.Book!.ToDomain(lb.AvailableCount))
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new LibraryRepositoryException($"There was an error getting the books of library with uuid = {bookRequest.LibraryUuid}.", e);
        }
    }

    public async Task<List<Library>> GetLibrariesByIdsAsync(List<Guid> ids)
    {
        try
        {
            var librariesDb = await _context.Libraries
                .Where(l => ids.Contains(l.LibraryUuid))
                .ToListAsync();

            return librariesDb.ConvertAll(l => l.ToDomain());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new LibraryRepositoryException($"There was an error getting the libraries by ids = {ids}.", e);
        }
    }

    public async Task<LibraryBook> ChangeBookCountAsync(Guid libraryId, Guid bookId, bool isIncrease)
    {
        try
        {
            var libraryBook = await _context.LibraryBooks
                .Include(lb => lb.Book)
                .Include(lb => lb.Library)
                .FirstAsync(lb => lb.Library!.LibraryUuid == libraryId && lb.Book!.BookUuid == bookId);

            if (isIncrease)
                libraryBook.AvailableCount++;
            else
                libraryBook.AvailableCount--;
            
            await _context.SaveChangesAsync();
            
            return libraryBook.ToDomain();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new LibraryRepositoryException($"There was an error decreasing the book count with id = {bookId} in library with id = {libraryId}.", e);
        }
    }
}