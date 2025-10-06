using RatingSystem.DataAccess.Models;
using RatingSystem.Web.Dto;

namespace RatingSystem.Tests.UnitTests.Factories;

public static class RatingDbFactory
{
    public static RatingDb Create(int id = 1,
        string username = "Username",
        int stars = 100)
    {
        return new RatingDb(id, username, stars);
    }
}