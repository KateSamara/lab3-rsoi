using GatewayService.Domain.Models.Ratings;

namespace GatewayService.Domain.Interfaces.Services;

public interface IRatingService
{
    public Task<Rating> GetRatingsByUsernameAsync(string username);
}