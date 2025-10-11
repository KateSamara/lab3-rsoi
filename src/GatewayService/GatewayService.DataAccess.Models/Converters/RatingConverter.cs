using GatewayService.DataAccess.Models.Ratings;
using GatewayService.Domain.Models.Ratings;

namespace GatewayService.DataAccess.Models.Converters;

public static class RatingConverter
{
    public static Rating ToDomain(this RatingDto ratingDto)
    {
        return new Rating
        {
            Stars = ratingDto.Stars
        };
    }
}