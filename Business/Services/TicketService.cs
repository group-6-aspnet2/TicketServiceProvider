using Azure.Messaging.ServiceBus;
using Data.Entities;
using Data.Interfaces;
using Domain.Extensions;
using Domain.Models;
using Domain.Responses;
using System.Diagnostics;
using System.Net.Sockets;

namespace Business.Services;

public interface ITicketService
{
    Task<TicketResponse<IEnumerable<TicketModel>>> CreateNewTicketsAsync(CreateTicketsForm form);
    Task<TicketResponse<IEnumerable<TicketModel>>> GetAllTicketsByUserIdAsync(string userId);
    Task<TicketResponse<IEnumerable<TicketModel>>> GetTicketsByBookingIdAsync(string bookingId);
}

public class TicketService(ITicketRepository ticketRepository /*, EventContract.EventContractClient eventClient */) : ITicketService
{
    private readonly ITicketRepository _ticketRepository = ticketRepository;
    //private readonly EventContract.EventContractClient _eventClient = eventClient;

    public async Task<TicketResponse<IEnumerable<TicketModel>>> GetAllTicketsByUserIdAsync(string userId)
    {
        var result = await _ticketRepository.GetAllAsync(filterBy: x => x.UserId == userId, sortByColumn: x => x.BookingId, orderByDescending: true);

        // Hämta users och events med UserId och EventId, returnera med properties i TicketModel


        return new TicketResponse<IEnumerable<TicketModel>> { Succeeded = true, Result = result.Result };
    }

    public async Task<TicketResponse<IEnumerable<TicketModel>>> GetTicketsByBookingIdAsync(string bookingId)
    {
        var result = await _ticketRepository.GetAllAsync(filterBy: x => x.BookingId == bookingId);

        // Hämta users och events med UserId och EventId, returnera med properties i TicketModel

        return new TicketResponse<IEnumerable<TicketModel>> { Succeeded = true, Result = result.Result };
    }

    public async Task<TicketResponse<IEnumerable<TicketModel>>> CreateNewTicketsAsync(CreateTicketsForm form)
    {
        try
        {
            if (form == null)
                return new TicketResponse<IEnumerable<TicketModel>> { Succeeded = false, Error = "Invalid ticket form", StatusCode = 400 };

            var entities = new List<TicketEntity>();

            for (int i = 0; i < form.TicketQuantity; i++)
            {
                var entityToAdd = new TicketEntity
                {
                    BookingId = form.BookingId,
                    EventId = form.EventId,
                    UserId = form.UserId,
                    TicketPrice = form.TicketPrice,
                    SeatNumber = "19B",
                    Gate = "C",
                    TicketCategoryName = form.TicketCategoryName
                };
                entities.Add(entityToAdd);
            }

            var results = new List<RepositoryResult<TicketModel>>();
            var models = new List<TicketModel>();

            //foreach (var ticket in entities)
            //{
            //    var result = await _ticketRepository.AddAsync(ticket);

            //    if (!result.Succeeded)
            //        results.Add(result);
                
            //    models.Add(result.Result!);
            //}

            for (int i = 0; i < entities.Count(); i++)
            {
                var result = await _ticketRepository.AddAsync(entities[i]);

                if (!result.Succeeded)
                    results.Add(result);

                models.Add(result.Result!);
            }

            if (results.Any(x => x.Succeeded == false))
                return new TicketResponse<IEnumerable<TicketModel>> { Succeeded = false, Error = "Failed to create tickets", StatusCode = 500 };

            //var eventRequest = new GetEventByIdRequest { EventId = form.EventId };
            //GetEventByIdReply eventReply = _eventClient.GetEventById(eventRequest);

            models.ForEach(ticket =>
            { // använd eventReply istället när det funkar
                ticket.EventName = "Way Out West";
                ticket.EventDate = DateOnly.FromDateTime(DateTime.Now);
                ticket.EventTime = TimeOnly.FromDateTime(DateTime.Now);
                ticket.EventLocation = "Jussi Björlings allé, 111 47 Stockholm";
                ticket.EventCategoryName = "Music";
            });

            return new TicketResponse<IEnumerable<TicketModel>> { Succeeded = true, StatusCode = 201, Result = models };

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new TicketResponse<IEnumerable<TicketModel>>
            {
                Succeeded = false,
                Error = ex.Message,
                StatusCode = 500,
            };
        }
    }

}
