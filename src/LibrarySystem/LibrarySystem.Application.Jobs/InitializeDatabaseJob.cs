using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Domain.Models;
using LibrarySystem.Domain.Models.Books;
using LibrarySystem.Domain.Models.Libraries;

namespace LibrarySystem.Application.Jobs;

public class InitializeDatabaseJob(IBookRepository bookRepository,
    ILibraryBookRepository libraryBookRepository,
    ILibraryRepository libraryRepository)
{
    private readonly IBookRepository _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
    private readonly ILibraryRepository _libraryRepository = libraryRepository ?? throw new ArgumentNullException(nameof(libraryRepository));
    private readonly ILibraryBookRepository _libraryBookRepository = libraryBookRepository ?? throw new ArgumentNullException(nameof(libraryBookRepository));
    
    public async Task InitializeDatabaseAsync()
    {
        try
        {
            if (await _bookRepository.GetBooksCountAsync() == 0)
                await InitBookAsync();
            
            if (await _libraryRepository.GetLibrariesCountAsync() == 0)
                await InitLibraryAsync();
            
            if (await _libraryBookRepository.GetLibraryBooksCountAsync() == 0)
                await InitLibraryBookAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task InitBookAsync()
    {
        var book = new Book
        {
            Id = 1,
            BookUuid = new Guid("f7cdc58f-2caf-4b15-9727-f89dcc629b27"),
            Name = "Краткий курс C++ в 7 томах",
            Author = "Бьерн Страуструп",
            Genre = "Научная фантастика",
            Condition = BookCondition.EXCELLENT
        };
        
        await _bookRepository.AddBookAsync(book);
    }

    private async Task InitLibraryAsync()
    {
        var library = new Library
        {
            Id = 1,
            LibraryUuid = new Guid("83575e12-7ce0-48ee-9931-51919ff3c9ee"),
            Name = "Библиотека имени 7 Непьющих",
            City = "Москва",
            Address = "2-я Бауманская ул., д.5, стр.1"
        };
        
        await _libraryRepository.AddLibraryAsync(library);
    }

    private async Task InitLibraryBookAsync()
    {
        var libraryBook = new LibraryBook
        {
            BookId = 1,
            LibraryId = 1,
            AvailableCount = 1,
        };
        
        await _libraryBookRepository.AddLibraryBookAsync(libraryBook);
    }
}