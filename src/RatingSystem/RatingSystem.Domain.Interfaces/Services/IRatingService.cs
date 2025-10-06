using RatingSystem.Domain.Models;

namespace RatingSystem.Domain.Interfaces.Services;

public interface IRatingService
{
    public Task<Rating> GetRatingByUsernameAsync(string username);
    
    public Task UpdateRatingAsync(string username, int starDifference);
}