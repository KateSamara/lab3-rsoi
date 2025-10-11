using GatewayService.Domain.Models.Ratings;
using GatewayService.Web.Dto.Ratings;

namespace GatewayService.Web.Dto.Converters;

public static class RatingDtoConverter
{
    public static RatingDto ToDto(this Rating rating)
    {
        return new RatingDto(stars: rating.Stars);
    }
}