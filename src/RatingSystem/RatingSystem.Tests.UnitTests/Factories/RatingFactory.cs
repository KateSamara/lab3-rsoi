using RatingSystem.Domain.Models;

namespace RatingSystem.Tests.UnitTests.Factories;

public static class RatingFactory
{
    public static Rating Create(int id = 1,
        string username = "Username", 
        int stars = 100)
    {
        return new Rating
        {
            Id = id,
            Username = username,
            Stars = stars
        };
    }
}