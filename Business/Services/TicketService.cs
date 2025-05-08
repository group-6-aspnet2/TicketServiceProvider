using Azure.Messaging.ServiceBus;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;
using Domain.Responses;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Text.Json;

namespace Business.Services;

public interface ITicketService
{
    Task<TicketResponse<IEnumerable<TicketModel>>> CreateNewTicketsAsync(CreateTicketsForm form);
    Task<TicketResponse<IEnumerable<TicketModel>>> GetAllTicketsAsync();
    Task<TicketResponse<IEnumerable<TicketModel>>> GetAllTicketsByUserIdAsync(string userId);
    Task<TicketResponse<IEnumerable<TicketModel>>> GetTicketsByBookingIdAsync(string bookingId);
    Task ListenAsync();
    Task StopListeningAsync();
}

public class TicketService : ITicketService
{
    //private readonly EventContract.EventContractClient _eventClient = eventClient;
    private readonly ITicketRepository _ticketRepository;
    private readonly ServiceBusClient _client;
    private ServiceBusProcessor? _processor;

    public TicketService(IConfiguration configuration, ITicketRepository ticketRepository, ServiceBusClient client)
    {
        _ticketRepository = ticketRepository;
        _client = new ServiceBusClient(configuration["ServiceBus:ConnectionString"]);
    }


    public async Task ListenAsync()
    {
        var processorOptions = new ServiceBusProcessorOptions();
        _processor = _client.CreateProcessor("create-ticket", processorOptions);

        _processor.ProcessMessageAsync += async args =>
        {
            var body = args.Message.Body.ToString();
            Console.WriteLine($"Received message: {body}");

            var form = JsonSerializer.Deserialize<CreateTicketsForm>(body);

            Console.WriteLine("Form i string: ", form!.ToString());
            if (!string.IsNullOrWhiteSpace(form.ToString()))
            {
                await CreateNewTicketsAsync(form);
                await args.CompleteMessageAsync(args.Message);
            }
        };

        _processor.ProcessErrorAsync += args =>
        {
            Console.WriteLine($"Message handler encountered an exception: {args.Exception.Message}");
            return Task.CompletedTask;
        };
        await _processor.StartProcessingAsync();
    }

    public async Task StopListeningAsync()
    {
        if (_processor != null)
        {
            await _processor.StopProcessingAsync();
            await _processor.DisposeAsync();
        }
    }


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
