using GatewayService.Application.Helpers.Queues;
using GatewayService.Domain.Interfaces.Gateways;
using GatewayService.Domain.Models.QueueTask;
using Microsoft.Extensions.Hosting;

namespace GatewayService.Application.Helpers.Workers;

public class LibraryWorker(TaskQueue<LibraryTask> libraryQueue, ILibraryGateway libraryGateway, TaskQueue<RatingTask> ratingQueue)
    : BackgroundService
{
    private readonly ILibraryGateway _libraryGateway = libraryGateway ?? throw new ArgumentNullException(nameof(libraryGateway));
    private readonly TaskQueue<LibraryTask> _libraryQueue = libraryQueue ?? throw new ArgumentNullException(nameof(libraryQueue));
    private readonly TaskQueue<RatingTask> _ratingQueue = ratingQueue ?? throw new ArgumentNullException(nameof(ratingQueue));
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("LibraryWorker запущен.");

        await foreach (var task in _libraryQueue.ReadAllAsync(stoppingToken))
        {
            try
            {
                var libraryBook = await _libraryGateway.UpdateAvailableBooksCount(task.Reservation.BookUuid,
                    task.Reservation.LibraryUuid, true);

                Console.WriteLine("Book available count was updated successfully");
                
                await _ratingQueue.EnqueueAsync(new RatingTask
                {
                    Username = task.Username,
                    Status = task.Reservation.Status,
                    OldCondition = libraryBook.Book.Condition,
                    NewCondition = task.ReservationDelete.Condition,
                });
            }
            catch (Exception)
            {
                Console.WriteLine("Ошибка при обновлении LibraryService. Повтор через 10 секунд.");
                await Task.Delay(10_000, stoppingToken);
                await _libraryQueue.EnqueueAsync(task);
            }
        }
    }
}