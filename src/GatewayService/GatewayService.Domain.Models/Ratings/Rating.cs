namespace GatewayService.Domain.Models.Ratings;

public record Rating
{
    public required int Stars { get; init; }
}