using Business.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController(ITicketService ticketService) : ControllerBase
    {
        private readonly ITicketService _ticketService = ticketService;

        [HttpGet]
        [SwaggerOperation(Summary = "Returns a list of tickets")]
        [SwaggerResponse(200, "List of tickets returned successfully")]
        [SwaggerResponse(500, "An error occurred while processing the request")]
        public async Task<IActionResult> GetAllTickets()
        {
            try
            {
                var result = await _ticketService.GetAllTicketsAsync();

                return result.StatusCode switch
                {
                    200 => Ok(result.Result),
                    _ => Problem(result.Error)
                };
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        [HttpGet("user/{userId}")]
        [SwaggerOperation(Summary = "Returns a list of tickets for a specific user")]
        [SwaggerResponse(200, "List of tickets for the user returned successfully")]
        [SwaggerResponse(400, "Invalid user ID provided")]
        [SwaggerResponse(500, "An error occurred while processing the request")]
        public async Task<IActionResult> GetTicketsByUserId(string userId)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(userId))
                    return BadRequest("No valid user id provided");

                var result = await _ticketService.GetAllTicketsByUserIdAsync(userId);
                return result.StatusCode switch
                {
                    200 => Ok(result.Result),
                    _ => Problem(result.Error)
                };
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        [HttpGet("event/{eventId}")]
        [SwaggerOperation(Summary = "Returns a list of tickets for a specific event")]
        [SwaggerResponse(200, "List of tickets for the event returned successfully")]
        [SwaggerResponse(400, "Invalid event ID provided")]
        [SwaggerResponse(500, "An error occurred while processing the request")]
        public async Task<IActionResult> GetTicketsByEventId(string eventId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(eventId))
                    return BadRequest("No valid event id provided");

                var result = await _ticketService.GetAllTicketsByEventIdAsync(eventId);
                return result.StatusCode switch
                {
                    200 => Ok(result.Result),
                    _ => Problem(result.Error)
                };
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        [HttpGet("booking/{bookingId}")]
        [SwaggerOperation(Summary = "Returns a list of tickets for a specific booking")]
        [SwaggerResponse(200, "List of tickets for the booking returned successfully")]
        [SwaggerResponse(400, "Invalid booking ID provided")]
        [SwaggerResponse(500, "An error occurred while processing the request")]
        public async Task<IActionResult> GetTicketsByBookingId(string bookingId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(bookingId))
                    return BadRequest("No valid booking id provided");

                var result = await _ticketService.GetTicketsByBookingIdAsync(bookingId);
                return result.StatusCode switch
                {
                    200 => Ok(result.Result),
                    _ => Problem(result.Error)
                };

            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }
    }
}
