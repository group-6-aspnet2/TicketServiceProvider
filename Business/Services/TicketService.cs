using Business.Factories;
using Business.Helpers;
using Data.Entities;
using Data.Interfaces;
using Domain.Extensions;
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
public class TicketService(ITicketRepository ticketRepository, EventContract.EventContractClient eventClient) : ITicketService
{
    private readonly ITicketRepository _ticketRepository = ticketRepository;
    private readonly EventContract.EventContractClient _eventClient = eventClient;
    public async Task<TicketResponse<IEnumerable<TicketModel>>> GetAllTicketsAsync()
    {
        var result = await _ticketRepository.GetAllAsync();

        if(result.Result == null || !result.Succeeded)
            return new TicketResponse<IEnumerable<TicketModel>>{ Error= "Could not get all tickets", StatusCode= result.StatusCode, Succeeded = result.Succeeded};

        var ticketModels = new List<TicketModel>();

        foreach (var ticket in result.Result)
        {
            var eventResult = await _eventClient.GetEventByIdAsync(new GetEventByIdRequest{EventId= ticket.EventId});
            var eventForTicket = eventResult.Event;

            var model = ticket.MapTo<TicketModel>();

            var modelWithEventDetails = TicketFactory.MapEventToTicketModel(model, eventForTicket);
            if (modelWithEventDetails != null)
                ticketModels.Add(modelWithEventDetails);
        }

        return new TicketResponse<IEnumerable<TicketModel>> { Succeeded = true, Result = ticketModels, StatusCode = result.StatusCode };
    }

    public async Task<TicketResponse<IEnumerable<TicketModel>>> GetAllTicketsByUserIdAsync(string userId)
    {
        var result = await _ticketRepository.GetAllAsync(filterBy: x => x.UserId == userId, sortByColumn: x => x.BookingId, orderByDescending: true);

        if (result.Result == null || !result.Succeeded)
            return new TicketResponse<IEnumerable<TicketModel>> { Error = "Could not get all tickets", StatusCode = result.StatusCode, Succeeded = result.Succeeded };

        var ticketModels = new List<TicketModel>();

        foreach (var ticket in result.Result)
        {
            var eventResult = await _eventClient.GetEventByIdAsync(new GetEventByIdRequest { EventId = ticket.EventId });
            var eventForTicket = eventResult.Event;

            var model = ticket.MapTo<TicketModel>();

            var modelWithEventDetails = TicketFactory.MapEventToTicketModel(model, eventForTicket);
            if (modelWithEventDetails != null)
                ticketModels.Add(modelWithEventDetails);
        }

        return new TicketResponse<IEnumerable<TicketModel>> { Succeeded = true, Result = ticketModels, StatusCode = result.StatusCode };
    }

    public async Task<TicketResponse<IEnumerable<TicketModel>>> GetTicketsByBookingIdAsync(string bookingId)
    {
        var result = await _ticketRepository.GetAllAsync(filterBy: x => x.BookingId == bookingId);

        if (result.Result == null || !result.Succeeded)
            return new TicketResponse<IEnumerable<TicketModel>> { Error = "Could not get all tickets", StatusCode = result.StatusCode, Succeeded = result.Succeeded };

        var ticketModels = new List<TicketModel>();

        foreach (var ticket in result.Result)
        {
            var eventResult = await _eventClient.GetEventByIdAsync(new GetEventByIdRequest { EventId = ticket.EventId });
            var eventForTicket = eventResult.Event;

            var model = ticket.MapTo<TicketModel>();

            var modelWithEventDetails = TicketFactory.MapEventToTicketModel(model, eventForTicket);
            if (modelWithEventDetails != null)
                ticketModels.Add(modelWithEventDetails);
        }
        return new TicketResponse<IEnumerable<TicketModel>> { Succeeded = true, Result = ticketModels, StatusCode=result.StatusCode };
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

            if (!result.Succeeded || result.Result == null)
                return new TicketResponse<IEnumerable<TicketModel>> { Succeeded = false, Error = "Failed to create tickets", StatusCode = 500 };

            var eventResult = await _eventClient.GetEventByIdAsync(new GetEventByIdRequest { EventId = form.EventId });
            var eventForTicket = eventResult.Event;



            foreach (var ticket in result.Result)
            {
                var model = ticket.MapTo<TicketModel>();
                var ticketWithEventDetails = TicketFactory.MapEventToTicketModel(ticket, eventForTicket);

                models.Add(model);
            }


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
