using Business.Services;
using Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController(ITicketService ticketService) : ControllerBase
    {
        private readonly ITicketService _ticketService = ticketService;

        [HttpGet]
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

        [HttpGet("booking/{bookingId}")]
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
