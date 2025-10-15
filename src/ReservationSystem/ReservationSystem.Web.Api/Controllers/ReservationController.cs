using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Domain.Interfaces.Services;
using ReservationSystem.Web.Dto;
using ReservationSystem.Web.Dto.Converters;

namespace ReservationSystem.Web.Controllers;

[ApiController]
[Route("/api/v1/reservations")]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ReservationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetReservationsByUsernameAsync([FromHeader(Name = "X-User-Name")] string username)
    {
        var reservations = await _reservationService.GetReservationsByUsernameAsync(username);
        
        return Ok(reservations.ConvertAll(r => r.ToDto()));
    }

    [HttpGet("{status}")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetReservationsCountByStatusAndUsernameAsync(
        [FromHeader(Name = "X-User-Name")] string username,
        [FromRoute] string status)
    {
        Enum.TryParse(status, ignoreCase: true, out Domain.Models.ReservationStatus statusEnum);
        var count = await _reservationService.GetReservationsCountByStatusAndUsernameAsync(statusEnum, username);
        
        return Ok(count);
    }

    [HttpPost]
    [ProducesResponseType(typeof(List<ReservationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateReservationAsync([FromHeader(Name = "X-User-Name")] string username, [FromBody] ReservationCreateDto reservationCreateDto)
    {
        var newReservation = await _reservationService.AddReservationAsync(reservationCreateDto.ToDomain(username));
        
        return Ok(newReservation.ToDto());
    }

    [HttpPatch("{uuid}")]
    [ProducesResponseType(typeof(List<ReservationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteReservationAsync([FromRoute] Guid uuid, [FromQuery] DateOnly returnDate)
    {
        var reservation = await _reservationService.DeleteReservationAsync(uuid, returnDate);
        if (reservation == null)
            return NotFound();
        
        return Ok(reservation.ToDto());
    }
    
    [HttpDelete("{uuid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteReservationAsync([FromRoute] Guid uuid)
    {
        await _reservationService.DeleteReservationAsync(uuid);
        
        return NoContent();
    }
}