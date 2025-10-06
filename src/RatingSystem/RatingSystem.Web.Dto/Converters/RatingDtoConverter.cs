using RatingSystem.Domain.Models;

namespace RatingSystem.Web.Dto.Converters;

public static class RatingDtoConverter
{
    public static RatingDto ToDto(this Rating rating)
    {
        return new RatingDto(stars: rating.Stars);
    }
}