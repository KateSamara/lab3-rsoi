using LibrarySystem.Domain.Models;

namespace LibrarySystem.Domain.Interfaces.Services;

public interface ILibraryBookService
{
    public Task<LibraryBook> GetLibraryBookByBookAndLibraryIdsAsync(Guid bookId, Guid libraryId);
}