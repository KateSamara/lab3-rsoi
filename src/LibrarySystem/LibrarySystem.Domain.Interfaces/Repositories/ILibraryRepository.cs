using LibrarySystem.Domain.Models;
using LibrarySystem.Domain.Models.Books;
using LibrarySystem.Domain.Models.Libraries;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface ILibraryRepository
{
    public Task<int> GetLibrariesCountAsync();
    
    public Task AddLibraryAsync(Library library);
    
    public Task<LibraryPaged> GetLibrariesPagedAsync(LibraryRequest libraryRequest);
    
    public Task<BookPaged> GetBookPagedByLibraryUuidAsync(BookRequest bookRequest);
    
    public Task<List<Library>> GetLibrariesByIdsAsync(List<Guid> ids);
    
    public Task<LibraryBook> ChangeBookCountAsync(Guid libraryId, Guid bookId, bool isIncrease);
}