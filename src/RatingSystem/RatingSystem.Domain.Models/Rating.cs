namespace RatingSystem.Domain.Models;

public record Rating
{
    public required int Id { get; init; }
    public required string Username { get; init; }
    public required int Stars { get; init; }
}