namespace GatewayService.Domain.Models.QueueTask;

public record RatingTask
{
    public required string Username { get; init; }
    public required string Status { get; init; }
    public required string OldCondition { get; init; }
    public required string NewCondition { get; init; }
}