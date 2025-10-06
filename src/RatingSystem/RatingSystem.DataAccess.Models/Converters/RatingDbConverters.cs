using RatingSystem.Domain.Models;

namespace RatingSystem.DataAccess.Models.Converters;

public static class RatingDbConverters
{
    public static Rating ToDomain(this RatingDb ratingDb)
    {
        return new Rating
        {
            Id = ratingDb.Id,
            Username = ratingDb.Username,
            Stars = ratingDb.Stars
        };
    }

    public static RatingDb ToDb(this Rating rating, int id)
    {
        return new RatingDb(id: id, username: rating.Username, stars: rating.Stars);
    }
}