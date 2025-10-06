using RatingSystem.Domain.Exceptions.Services;
using RatingSystem.Domain.Interfaces.Repositories;
using RatingSystem.Domain.Interfaces.Services;
using RatingSystem.Domain.Models;

namespace RatingSystem.Application.Services;

public class RatingService(IRatingRepository ratingRepository) : IRatingService
{
    private readonly IRatingRepository _ratingRepository = ratingRepository ?? throw new ArgumentNullException(nameof(ratingRepository));

    public async Task<Rating> GetRatingByUsernameAsync(string username)
    {
        try
        {
            return await _ratingRepository.GetRatingByUsernameAsync(username);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new RatingServiceException($"There was an error while getting the rating by username = {username}.",
                e);
        }
    }

    public async Task UpdateRatingAsync(string username, int starDifference)
    {
        try
        {
            await _ratingRepository.UpdateRatingAsync(username, starDifference);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new RatingServiceException($"There was an error while updating the rating for user = {username}.", e);
        }
    }
}