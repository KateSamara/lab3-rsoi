using LibrarySystem.Domain.Exceptions.Services;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Domain.Interfaces.Services;
using LibrarySystem.Domain.Models;

namespace LibrarySystem.Application.Services;

public class LibraryBookService(ILibraryBookRepository libraryBookRepository) : ILibraryBookService
{
    private readonly ILibraryBookRepository _libraryBookRepository = libraryBookRepository ?? throw new ArgumentNullException(nameof(libraryBookRepository));

    public async Task<LibraryBook> GetLibraryBookByBookAndLibraryIdsAsync(Guid bookId, Guid libraryId)
    {
        try
        {
            return await _libraryBookRepository.GetLibraryBookByBookAndLibraryIdsAsync(bookId, libraryId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new LibraryBookServiceException(
                $"There was an error getting the library book by book id = {bookId} and library id = {libraryId}.", e);
        }
    }
}