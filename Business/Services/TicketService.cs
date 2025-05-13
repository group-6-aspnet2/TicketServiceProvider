using Business.Helpers;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;
using Domain.Responses;
using System.Diagnostics;

namespace Business.Services;

public interface ITicketService
{
    Task<TicketResponse<IEnumerable<TicketModel>>> CreateNewTicketsAsync(CreateTicketsForm form);
    Task<TicketResponse<IEnumerable<TicketModel>>> GetAllTicketsAsync();
    Task<TicketResponse<IEnumerable<TicketModel>>> GetAllTicketsByUserIdAsync(string userId);
    Task<TicketResponse<IEnumerable<TicketModel>>> GetTicketsByBookingIdAsync(string bookingId);
}
    //private readonly EventContract.EventContractClient _eventClient = eventClient;
public class TicketService(ITicketRepository ticketRepository) : ITicketService
{
    private readonly ITicketRepository _ticketRepository = ticketRepository;
    
    public async Task<TicketResponse<IEnumerable<TicketModel>>> GetAllTicketsAsync()
    {
        var result = await _ticketRepository.GetAllAsync();
        // Hämta event med EventId, returnera med properties i TicketModel
        return new TicketResponse<IEnumerable<TicketModel>> { Succeeded = true, Result = result.Result };
    }

    public async Task<TicketResponse<IEnumerable<TicketModel>>> GetAllTicketsByUserIdAsync(string userId)
    {
        var result = await _ticketRepository.GetAllAsync(filterBy: x => x.UserId == userId, sortByColumn: x => x.BookingId, orderByDescending: true);

        // Hämta event med EventId, returnera med properties i TicketModel


        return new TicketResponse<IEnumerable<TicketModel>> { Succeeded = true, Result = result.Result };
    }

    public async Task<TicketResponse<IEnumerable<TicketModel>>> GetTicketsByBookingIdAsync(string bookingId)
    {
        var result = await _ticketRepository.GetAllAsync(filterBy: x => x.BookingId == bookingId);

        // Hämta event med EventId, returnera med properties i TicketModel

        return new TicketResponse<IEnumerable<TicketModel>> { Succeeded = true, Result = result.Result };
    }

    public async Task<TicketResponse<IEnumerable<TicketModel>>> CreateNewTicketsAsync(CreateTicketsForm form)
    {
        try
        {
            if (form == null)
                return new TicketResponse<IEnumerable<TicketModel>> { Succeeded = false, Error = "Invalid ticket form", StatusCode = 400 };

            var entities = new List<TicketEntity>();
            var voucherInfos = TicketGenerator.GenerateSeatsAndGate(form.TicketQuantity);
            
            for (int i = 0; i < form.TicketQuantity; i++)
            {
                if (voucherInfos.Count != form.TicketQuantity)
                    return new TicketResponse<IEnumerable<TicketModel>> { Succeeded = false, Error = "Failed to generate tickets", StatusCode = 500 };

                var entityToAdd = new TicketEntity
                {
                    BookingId = form.BookingId,
                    EventId = form.EventId,
                    UserId = form.UserId,
                    TicketPrice = form.TicketPrice,
                    SeatNumber = voucherInfos[i].SeatNumber,
                    Gate = voucherInfos[i].Gate,
                    TicketCategoryName = form.TicketCategoryName
                };

                entities.Add(entityToAdd);
            }
         
              var results = new List<RepositoryResult<TicketModel>>();
              var models = new List<TicketModel>();

            var result = await _ticketRepository.AddRangeAsync(entities);
         
            if(!result.Succeeded)
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
