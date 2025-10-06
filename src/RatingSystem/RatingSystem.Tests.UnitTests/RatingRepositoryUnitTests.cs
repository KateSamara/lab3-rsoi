using Moq;
using Moq.EntityFrameworkCore;
using RatingSystem.DataAccess.Context;
using RatingSystem.DataAccess.Repositories;
using RatingSystem.Domain.Interfaces.Repositories;
using RatingSystem.Tests.UnitTests.Factories;

namespace RatingSystem.Tests.UnitTests;

public class RatingRepositoryUnitTests
{
    private readonly IRatingRepository _ratingRepository;
    private readonly Mock<RatingSystemContext> _mockContext = new();

    public RatingRepositoryUnitTests()
    {
        _ratingRepository = new RatingRepository(_mockContext.Object);
    }
    
    [Fact]
    public async Task GetRatingByUsernameAsync_Basic_Ok()
    {
        // Arrange
        var username = "Max";
        _mockContext.Setup(c => c.Ratings)
            .ReturnsDbSet([RatingDbFactory.Create(username: username), RatingDbFactory.Create()]);
        
        var expectedRating = RatingFactory.Create(username: username);
        
        // Act
        var actualRating = await _ratingRepository.GetRatingByUsernameAsync(username);
        
        // Assert
        Assert.Equal(expectedRating, actualRating);
    }

    [Fact]
    public async Task GetRatingCountAsync_Basic_Ok()
    {
        // Arrange
        _mockContext.Setup(c => c.Ratings)
            .ReturnsDbSet([RatingDbFactory.Create(), RatingDbFactory.Create(), RatingDbFactory.Create()]);
        
        var expectedCount = 3;
        
        // Act
        var actualCount = await _ratingRepository.GetRatingCountAsync();
        
        // Assert
        Assert.Equal(expectedCount, actualCount);
    }
}