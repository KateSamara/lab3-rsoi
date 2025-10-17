using GatewayService.Domain.Exceptions.Gateways;
using GatewayService.Domain.Exceptions.Services;
using GatewayService.Domain.Interfaces.Gateways;
using GatewayService.Domain.Interfaces.Services;
using GatewayService.Domain.Models.Ratings;

namespace GatewayService.Application.Services;

public class RatingService(IRatingGateway ratingGateway) : IRatingService
{
    private readonly IRatingGateway _ratingGateway = ratingGateway ?? throw new ArgumentNullException(nameof(ratingGateway));

    public async Task<Rating> GetRatingsByUsernameAsync(string username)
    {
        try
        {
            return await _ratingGateway.GetRatingsByUsernameAsync(username);
        }
        catch (RatingServiceNotAvailableGatewayException)
        {
            Console.WriteLine("Rating service not available.");
            throw new RatingServiceNotAvailableServiceException("Rating service not available.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to get rating by username = {username}", e);
            throw new RatingServiceException($"Failed to get rating by username = {username}", e);
        }
    }
} 