using GatewayService.Domain.Models.Ratings;

namespace GatewayService.Domain.Interfaces.Gateways;

public interface IRatingGateway
{
    public Task<Rating> GetRatingsByUsernameAsync(string username);

    public Task UpdateRatingAsync(string username, int starDifference);
}