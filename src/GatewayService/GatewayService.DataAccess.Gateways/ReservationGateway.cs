using System.Net;
using System.Text;
using System.Text.Json;
using GatewayService.DataAccess.Gateways.CircuitBreakers;
using GatewayService.DataAccess.Gateways.Configuration;
using GatewayService.DataAccess.Models.Converters;
using GatewayService.DataAccess.Models.Reservations;
using GatewayService.Domain.Exceptions.Gateways;
using GatewayService.Domain.Interfaces.Gateways;
using GatewayService.Domain.Models.Reservations;
using Microsoft.Extensions.Options;

namespace GatewayService.DataAccess.Gateways;

public class ReservationGateway(IOptions<ReservationSystemConfiguration> reservationSystemConfiguration,
    CircuitBreaker<ReservationGateway> circuitBreaker) : IReservationGateway
{
    private readonly ReservationSystemConfiguration _reservationSystemConfiguration = reservationSystemConfiguration.Value ?? throw new ArgumentNullException(nameof(reservationSystemConfiguration));
    private readonly CircuitBreaker<ReservationGateway> _reservationCircuitBreaker = circuitBreaker ?? throw new ArgumentNullException(nameof(circuitBreaker));
    
    public async Task<int> GetCurrentReservationsCountByUsernameAsync(string username)
    {
        try
        {
            using var client = new HttpClient();
            
            using var request = new HttpRequestMessage(HttpMethod.Get,
                $"{_reservationSystemConfiguration.IpAddress}/{_reservationSystemConfiguration.BaseUrl}/RENTED");
            request.Headers.Add(_reservationSystemConfiguration.UsernameHeader, username);

            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var reservationCountResponseString = await response.Content.ReadAsStringAsync();
        
            var currentReservationsCount = int.Parse(reservationCountResponseString);
        
            return currentReservationsCount;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to get reservations count for user {username}", e);
            throw new ReservationServiceNotAvailableGatewayException($"Failed to get reservations count for user {username}", e);
        }
    }

    public async Task<ReservationShort> AddReservationAsync(string username, ReservationCreate reservationCreate)
    {
        try
        {
            using var client = new HttpClient();
            
            var content = JsonSerializer.Serialize(reservationCreate.ToDto());
        
            using var request = new HttpRequestMessage(HttpMethod.Post,
                $"{_reservationSystemConfiguration.IpAddress}/{_reservationSystemConfiguration.BaseUrl}");
            request.Headers.Add(_reservationSystemConfiguration.UsernameHeader, username);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var newReservationJson = await response.Content.ReadAsStringAsync();
        
            var newReservation = JsonSerializer.Deserialize<ReservationShortDto>(newReservationJson);
        
            return newReservation!.ToDomain();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to add reservation for {username}", e);
            throw new ReservationServiceNotAvailableGatewayException($"Failed to add reservation for {username}", e);
        }
    }

    public async Task<List<ReservationShort>> GetReservationsByUsernameAsync(string username)
    {
        return await _reservationCircuitBreaker.ExecuteAsync(
            action: async () => await CallGetReservationsByUsernameAsync(username),
            fallbackAction: () =>
            {
                Console.WriteLine("Reservation service is unavailable.");
                throw new ReservationServiceNotAvailableGatewayException("Reservation service is unavailable.");
            },
            checkHealthAction: async () => await IsReservationServiceAvailableAsync()
        );
    }

    private async Task<List<ReservationShort>> CallGetReservationsByUsernameAsync(string username)
    {
        try
        {
            using var client = new HttpClient();
            
            using var request = new HttpRequestMessage(HttpMethod.Get,
                $"{_reservationSystemConfiguration.IpAddress}/{_reservationSystemConfiguration.BaseUrl}");
            request.Headers.Add(_reservationSystemConfiguration.UsernameHeader, username);

            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
        
            var reservations = JsonSerializer.Deserialize<List<ReservationShortDto>>(json);

            return reservations!.ConvertAll(r => r.ToDomain());
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to get reservations for {username}", e);
            throw new ReservationServiceNotAvailableGatewayException($"Failed to get reservations for {username}", e);
        }
    }

    public async Task<ReservationShort?> DeleteReservationAsync(Guid reservationId, DateOnly date)
    {
        try
        {
            using var client = new HttpClient();
            
            using var request = new HttpRequestMessage(HttpMethod.Patch,
                $"{_reservationSystemConfiguration.IpAddress}/{_reservationSystemConfiguration.BaseUrl}/{reservationId}" +
                $"?returnDate={date.ToString("yyyy.MM.dd")}");

            using var response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
        
            var reservation = JsonSerializer.Deserialize<ReservationShortDto>(json);

            return reservation!.ToDomain();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to delete reservation with id = {reservationId}", e);
            throw new ReservationServiceNotAvailableGatewayException($"Failed to delete reservation with id = {reservationId}", e);
        }
    }

    public async Task DeleteReservationAsync(Guid reservationId)
    {
        try
        {
            using var client = new HttpClient();
            
            using var request = new HttpRequestMessage(HttpMethod.Delete,
                $"{_reservationSystemConfiguration.IpAddress}/{_reservationSystemConfiguration.BaseUrl}/{reservationId}");

            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to rollback reservation with id = {reservationId}", e);
            throw new ReservationServiceNotAvailableGatewayException($"Failed to rollback reservation with id = {reservationId}", e);
        }
    }
    
    private async Task<bool> IsReservationServiceAvailableAsync()
    {
        try
        {
            using var client = new HttpClient();
        
            using var request = new HttpRequestMessage(HttpMethod.Get,
                $"{_reservationSystemConfiguration.IpAddress}/{_reservationSystemConfiguration.CheckHealth}");
        
            using var response = await client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }
}