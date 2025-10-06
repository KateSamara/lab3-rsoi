using LibrarySystem.Domain.Exceptions.Services;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Domain.Interfaces.Services;
using LibrarySystem.Domain.Models;
using LibrarySystem.Domain.Models.Books;
using LibrarySystem.Domain.Models.Libraries;

namespace LibrarySystem.Application.Services;

public class LibraryService(ILibraryRepository libraryRepository) : ILibraryService
{
    private readonly ILibraryRepository _libraryRepository = libraryRepository ?? throw new ArgumentNullException(nameof(libraryRepository));

    public async Task<LibraryPaged> GetLibrariesPagedAsync(LibraryRequest libraryRequest)
    {
        try
        {
            return await _libraryRepository.GetLibrariesPagedAsync(libraryRequest);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new LibraryServiceException("An error occured while getting all libraries.", e);
        }
    }

    public async Task<BookPaged> GetBookPagedByLibraryUuidAsync(BookRequest bookRequest)
    {
        try
        {
            return await _libraryRepository.GetBookPagedByLibraryUuidAsync(bookRequest);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new LibraryServiceException($"There was an error getting the books of library with uuid = {bookRequest.LibraryUuid}.", e);
        }
    }

    public async Task<List<Library>> GetLibrariesByIdsAsync(List<Guid> ids)
    {
        try
        {
            return await _libraryRepository.GetLibrariesByIdsAsync(ids);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new LibraryServiceException($"There was an error getting the libraries by ids = {ids}.", e);
        }
    }

    public async Task<LibraryBook> ChangeBookCountAsync(Guid libraryId, Guid bookId, bool isIncrease)
    {
        try
        {
            return await _libraryRepository.ChangeBookCountAsync(libraryId, bookId, isIncrease);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new LibraryServiceException(
                $"There was an error decreasing the book count with id = {bookId} in library with id = {libraryId}.",
                e);
        }
    }
}