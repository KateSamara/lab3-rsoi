using LibrarySystem.DataAccess.Context;
using LibrarySystem.DataAccess.Models;
using LibrarySystem.DataAccess.Repositories;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Domain.Models.Books;
using LibrarySystem.Tests.UnitTests.Factories;
using Moq;
using Moq.EntityFrameworkCore;

namespace LibrarySystem.Tests.UnitTests;

public class BookRepositoryUnitTests
{
    private readonly IBookRepository _bookRepository;
    private readonly Mock<LibrarySystemContext> _mockContext = new();

    public BookRepositoryUnitTests()
    {
        _bookRepository = new BookRepository(_mockContext.Object);
    }
    
    [Fact]
    public async Task GetBooksCountAsync_Basic_Ok()
    {
        // Arrange
        _mockContext.Setup(c => c.Books)
            .ReturnsDbSet([BookDbFactory.Create(), BookDbFactory.Create()]);
        var expectedCount = 2;
        
        // Act
        var actualCount = await _bookRepository.GetBooksCountAsync();
        
        // Assert
        Assert.Equal(expectedCount, actualCount);
    }

    [Fact]
    public async Task GetBooksByIdsAsync_Basic_Ok()
    {
        // Arrange
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        
        List<BookDb> booksDb = [BookDbFactory.Create(bookUuid: id1), BookDbFactory.Create(), BookDbFactory.Create(bookUuid: id2)];
        _mockContext.Setup(c => c.Books)
            .ReturnsDbSet(booksDb);

        List<Book> expectedBooks = [BookFactory.Create(bookUuid: id1), BookFactory.Create(bookUuid: id2)];
        
        // Act
        var actualBooks = await _bookRepository.GetBooksByIdsAsync([id1, id2]);
        
        // Assert
        Assert.Equal(expectedBooks, actualBooks);
    }
}