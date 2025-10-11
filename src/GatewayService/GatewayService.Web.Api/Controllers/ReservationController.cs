using GatewayService.Domain.Interfaces.Services;
using GatewayService.Web.Dto.Converters;
using GatewayService.Web.Dto.Reservations;
using Microsoft.AspNetCore.Mvc;

namespace GatewayService.Web.Api.Controllers;

[ApiController]
[Route("/api/v1/reservations")]
[ServiceFilter(typeof(ValidationFilterAttribute))]
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

    [HttpPost]
    [ProducesResponseType(typeof(List<ReservationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateReservationAsync([FromHeader(Name = "X-User-Name")] string username,
        [FromBody] ReservationCreateDto reservationCreate)
    {
        var newReservation = await _reservationService.CreateReservationAsync(username, reservationCreate.ToDomain());
        
        if (newReservation == null)
            return StatusCode(StatusCodes.Status403Forbidden);
        
        return Ok(newReservation.ToDto());
    }

    [HttpPost("{reservationId}/return")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteReservationAsync([FromHeader(Name = "X-User-Name")] string username,
        [FromRoute] Guid reservationId,
        [FromBody] ReservationDeleteDto reservationDelete)
    {
        var isDeleted = await _reservationService.DeleteReservationAsync(username, reservationId, reservationDelete.ToDomain());
        if (!isDeleted)
            return StatusCode(StatusCodes.Status404NotFound, new { message = "Reservation not found." });
        
        return NoContent();
    }
}