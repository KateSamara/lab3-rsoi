using GatewayService.Application.Helpers.Queues;
using GatewayService.Domain.Interfaces.Gateways;
using GatewayService.Domain.Models.QueueTask;
using Microsoft.Extensions.Hosting;

namespace GatewayService.Application.Helpers.Workers;

public class RatingWorker(IRatingGateway ratingGateway, TaskQueue<RatingTask> ratingQueue) : BackgroundService
{
    private readonly IRatingGateway _ratingGateway = ratingGateway ?? throw new ArgumentNullException(nameof(ratingGateway));
    private readonly TaskQueue<RatingTask> _ratingQueue = ratingQueue ?? throw new ArgumentNullException(nameof(ratingQueue));
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("RatingWorker запущен.");

        await foreach (var task in _ratingQueue.ReadAllAsync(stoppingToken))
        {
            try
            {
                var starDifference = 0;
                if (task.Status != "EXPIRED" && task.OldCondition == task.NewCondition)
                {
                    starDifference++;
                }
                else
                {
                    if (task.Status == "EXPIRED")
                        starDifference -= 10;
                    if (task.OldCondition != task.NewCondition)
                        starDifference -= 10;
                }
                
                await _ratingGateway.UpdateRatingAsync(task.Username, starDifference);
                
                Console.WriteLine("Rating was updated successfully");
            }
            catch (Exception)
            {
                Console.WriteLine("Ошибка при обновлении RatingService. Повтор через 10 секунд.");
                await Task.Delay(10_000, stoppingToken);
                await _ratingQueue.EnqueueAsync(task);
            }
        }
    }
}