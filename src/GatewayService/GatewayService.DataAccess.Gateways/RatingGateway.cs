using System.Text.Json;
using GatewayService.DataAccess.Gateways.CircuitBreakers;
using GatewayService.DataAccess.Gateways.Configuration;
using GatewayService.DataAccess.Models.Converters;
using GatewayService.DataAccess.Models.Ratings;
using GatewayService.Domain.Exceptions.Gateways;
using GatewayService.Domain.Interfaces.Gateways;
using GatewayService.Domain.Models.Ratings;
using Microsoft.Extensions.Options;

namespace GatewayService.DataAccess.Gateways;

public class RatingGateway(IOptions<RatingSystemConfiguration> ratingSystemConfiguration,
    CircuitBreaker<RatingGateway> circuitBreaker) : IRatingGateway
{
    private readonly RatingSystemConfiguration _ratingSystemConfiguration = ratingSystemConfiguration.Value ?? throw new ArgumentNullException(nameof(ratingSystemConfiguration));
    private readonly CircuitBreaker<RatingGateway> _ratingCircuitBreaker = circuitBreaker ?? throw new ArgumentNullException(nameof(circuitBreaker));

    public async Task<Rating> GetRatingsByUsernameAsync(string username)
    {
        return await _ratingCircuitBreaker.ExecuteAsync(
            action: async () => await CallGetRatingsByUsernameAsync(username),
            fallbackAction: () =>
            {
                Console.WriteLine("Rating service is unavailable.");
                throw new RatingServiceNotAvailableGatewayException("Rating service is unavailable.");
            },
            checkHealthAction: async () => await IsRatingServiceAvailableAsync()
        );
    }
    
    private async Task<Rating> CallGetRatingsByUsernameAsync(string username)
    {
        try
        {
            using var client = new HttpClient();

            using var request = new HttpRequestMessage(HttpMethod.Get,
                $"{_ratingSystemConfiguration.IpAddress}/{_ratingSystemConfiguration.BaseUrl}");
            request.Headers.Add(_ratingSystemConfiguration.UsernameHeader, username);

            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
        
            var rating = JsonSerializer.Deserialize<RatingDto>(json);

            return rating!.ToDomain();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to get rating by username = {username}", e);
            throw new RatingServiceNotAvailableGatewayException($"Failed to get rating by username = {username}", e);
        }
    }

    public async Task UpdateRatingAsync(string username, int starDifference)
    {
        try
        {
            using var client = new HttpClient();
            
            using var request = new HttpRequestMessage(HttpMethod.Patch,
                $"{_ratingSystemConfiguration.IpAddress}/{_ratingSystemConfiguration.BaseUrl}?starDifference={starDifference}");
            request.Headers.Add(_ratingSystemConfiguration.UsernameHeader, username);

            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to update rating by username = {username}", e);
            throw new RatingServiceNotAvailableGatewayException($"Failed to update rating by username = {username}", e);
        }
    }
    
    private async Task<bool> IsRatingServiceAvailableAsync()
    {
        try
        {
            using var client = new HttpClient();
        
            using var request = new HttpRequestMessage(HttpMethod.Get,
                $"{_ratingSystemConfiguration.IpAddress}/{_ratingSystemConfiguration.CheckHealth}");
        
            using var response = await client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }
}