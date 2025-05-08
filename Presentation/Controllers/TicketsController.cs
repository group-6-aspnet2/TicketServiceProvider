using Business.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TicketsController(ITicketService ticketService) : ControllerBase
{
    private readonly ITicketService _ticketService = ticketService;

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetAllTicketsByUserId(string userId)
    {

        try
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("UserId is required");

            var result = await _ticketService.GetAllTicketsByUserIdAsync(userId);
            return result.StatusCode switch
            {
                200 => Ok(result.Result),
                404 => NotFound(result.Error),
                _ => Problem(result.Error)
            };
        }
        catch (Exception ex)
        {
            Debug.Write(ex);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{bookingId}")]
    public async Task<IActionResult> GetTicketsByBookingId(string bookingId)
    {

        try
        {
            if(string.IsNullOrEmpty(bookingId))
                return BadRequest("BookingId is required");

            var result = await _ticketService.GetTicketsByBookingIdAsync(bookingId);
            return result.StatusCode switch
            {
                200 => Ok(result.Result),
                404 => NotFound(result.Error),
                _ => Problem(result.Error)
            };
        }
        catch (Exception ex)
        {
            Debug.Write(ex);
            return BadRequest(ex.Message);
        }
    }

    // Test för att kolla om service funkar
    //[HttpPost]
    //public async Task<IActionResult> CreateNewTickets(CreateTicketsForm form)
    //{
    //    try
    //    {
    //        if (form == null)
    //            return BadRequest("Invalid ticket form");


    //        var result = await _ticketService.CreateNewTicketsAsync(form);
    //        return result.StatusCode switch
    //        {
    //            201 => Ok(result.Result),
    //            400 => BadRequest(result.Error),
    //            _ => Problem(result.Error)
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.Write(ex);
    //        return BadRequest(ex.Message);
    //    }
    //}
}
