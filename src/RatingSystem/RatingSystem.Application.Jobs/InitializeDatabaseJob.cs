using RatingSystem.Domain.Interfaces.Repositories;
using RatingSystem.Domain.Models;

namespace RatingSystem.Application.Jobs;

public class InitializeDatabaseJob(IRatingRepository ratingRepository)
{
    private readonly IRatingRepository _ratingRepository = ratingRepository ?? throw new ArgumentNullException(nameof(ratingRepository));

    public async Task InitializeDatabaseAsync()
    {
        try
        {
            if (await _ratingRepository.GetRatingCountAsync() == 0)
                await InitRatingAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task InitRatingAsync()
    {
        var rating = new Rating
        {
            Id = 1,
            Stars = 75,
            Username = "Test Max"
        };
        
        await _ratingRepository.AddRatingAsync(rating);
    }
}