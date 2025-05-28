using Business;
using Business.Services;
using Business_Tests.TestHelpers;
using Data.Entities;
using Data.Interfaces;
using Domain.Extensions;
using Domain.Models;
using Domain.Responses;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Business_Tests.Services;

public class TicketServices_Tests
{

    private readonly Mock<ITicketRepository> _ticketRepository = new();
    private readonly Mock<EventContract.EventContractClient> _eventClient = new();
    private readonly ITicketService _ticketService;

    public TicketServices_Tests()
    {
        _ticketService = new TicketService(_ticketRepository.Object, _eventClient.Object);
    }



    [Fact]
    public async Task GetAllTicketsAsync_ShouldReturnTickets_WhenTicketsExist()
    {
        var ticketModels = new List<TicketModel>
    {
        new TicketModel
        {
            Id = "1",
            UserId = "user1",
            BookingId = "book1",
            EventId = "event1",
            TicketCategoryName = "VIP",
            TicketPrice = 250,
            SeatNumber = "12C",
            Gate = "B"
        }
    };

        var repositoryResult = new RepositoryResult<IEnumerable<TicketModel>>
        {
            Succeeded = true,
            Result = ticketModels,
            StatusCode = 200
        };

        _ticketRepository.Setup(r => r.GetAllAsync(
            false,
            It.IsAny<Expression<Func<TicketEntity, object>>>(),
            null,
            0,
            Array.Empty<Expression<Func<TicketEntity, object>>>()
        ))
        .ReturnsAsync(repositoryResult);

        var eventData = new Event
        {
            EventId = "event1",
            EventName = "Super Concert",
            EventCategoryName = "Music",
            EventLocation = "Big Arena",
            EventDate = "2025-05-28",
            EventTime = "18:19",
            EventStatus = "Live",
            EventAmountOfGuests = 10000
        };

        var eventResponse = new GetEventByIdReply
        {
            Event = eventData
        };

        _eventClient
            .Setup(c => c.GetEventByIdAsync(It.IsAny<GetEventByIdRequest>(), null, null, default))
            .Returns(GrpcTestHelpers.CreateAsyncUnaryCall(eventResponse));

        var result = await _ticketService.GetAllTicketsAsync();

        Assert.True(result.Succeeded);
        Assert.NotNull(result.Result);
        Assert.Single(result.Result);

        var ticket = Assert.Single(result.Result);
        Assert.Equal("user1", ticket.UserId);
        Assert.Equal("book1", ticket.BookingId);
        Assert.Equal("event1", ticket.EventId);
        Assert.Equal("VIP", ticket.TicketCategoryName);
        Assert.Equal(250, ticket.TicketPrice);
        Assert.Equal("12C", ticket.SeatNumber);
        Assert.Equal("B", ticket.Gate);

        Assert.Equal("Super Concert", ticket.EventName);
        Assert.Equal("Music", ticket.EventCategoryName);
        Assert.Equal("Big Arena", ticket.EventLocation);
        Assert.Equal(new DateOnly(2025, 5, 28), ticket.EventDate);
        Assert.Equal(new TimeOnly(18, 19), ticket.EventTime);
    }

    [Fact]
    public async Task GetAllTicketsByEventIdAsync_ShouldReturnTickets_WhenTicketsExist()
    {
        var eventId = "event1";

        var ticketModels = new List<TicketModel>
{
    new TicketModel
    {
        Id = "1",
        BookingId = "book1",
        UserId = "user1",
        EventId = eventId,
        TicketCategoryName = "VIP",
        TicketPrice = 100,
        SeatNumber = "1A",
        Gate = "A"
    }
};
        var repositoryResult = new RepositoryResult<IEnumerable<TicketModel>>
        {
            Succeeded = true,
            Result = ticketModels,
            StatusCode = 200
        };

        _ticketRepository.Setup(r => r.GetAllAsync(
         It.IsAny<bool>(),
         It.IsAny<Expression<Func<TicketEntity, object>>>(),
         It.IsAny<Expression<Func<TicketEntity, bool>>>(),
         It.IsAny<int>(),
         It.IsAny<Expression<Func<TicketEntity, object>>[]>()
     ))
     .ReturnsAsync(repositoryResult);


        var eventData = new Event
        {
            EventId = eventId,
            EventName = "Concert",
            EventCategoryName = "Music",
            EventLocation = "Arena",
            EventDate = "2025-05-28",
            EventTime = "20:00",
            EventStatus = "Active",
            EventAmountOfGuests = 5000
        };

        var grpcReply = new GetEventByIdReply { Event = eventData };

        _eventClient
            .Setup(c => c.GetEventByIdAsync(It.Is<GetEventByIdRequest>(req => req.EventId == eventId), null, null, default))
            .Returns(GrpcTestHelpers.CreateAsyncUnaryCall(grpcReply));

        var response = await _ticketService.GetAllTicketsByEventIdAsync(eventId);

        Assert.True(response.Succeeded);
        Assert.NotNull(response.Result);
        var ticket = Assert.Single(response.Result);

        Assert.Equal("book1", ticket.BookingId);
        Assert.Equal("user1", ticket.UserId);
        Assert.Equal(eventId, ticket.EventId);
        Assert.Equal("VIP", ticket.TicketCategoryName);
        Assert.Equal(100, ticket.TicketPrice);
        Assert.Equal("1A", ticket.SeatNumber);
        Assert.Equal("A", ticket.Gate);

        Assert.Equal("Concert", ticket.EventName);
        Assert.Equal("Music", ticket.EventCategoryName);
        Assert.Equal("Arena", ticket.EventLocation);
        Assert.Equal(new DateOnly(2025, 5, 28), ticket.EventDate);
        Assert.Equal(new TimeOnly(20, 0), ticket.EventTime);
    }


    [Fact]
    public async Task GetAllTicketsByUserIdAsync_ShouldReturnTickets_WhenTicketsExist()
    {
        var userId = "user1";
        var ticketModels = new List<TicketModel>
{
   new TicketModel
            {
                Id = "2",
                BookingId = "book2",
                UserId = userId,
                EventId = "event2",
                TicketCategoryName = "Standard",
                TicketPrice = 50,
                SeatNumber = "2B",
                Gate = "C"
            }
};
        var repositoryResult = new RepositoryResult<IEnumerable<TicketModel>>
        {
            Succeeded = true,
            Result = ticketModels,
            StatusCode = 200
        };

        _ticketRepository.Setup(r => r.GetAllAsync(
         It.IsAny<bool>(),
         It.IsAny<Expression<Func<TicketEntity, object>>>(),
         It.IsAny<Expression<Func<TicketEntity, bool>>>(),
         It.IsAny<int>(),
         It.IsAny<Expression<Func<TicketEntity, object>>[]>()
     ))
     .ReturnsAsync(repositoryResult);


        var eventData = new Event
        {
            EventId = "event2",
            EventName = "Theatre Play",
            EventCategoryName = "Drama",
            EventLocation = "Stage",
            EventDate = "2025-06-01",
            EventTime = "19:30",
            EventStatus = "Active",
            EventAmountOfGuests = 300
        };

        var grpcReply = new GetEventByIdReply { Event = eventData };

        _eventClient
            .Setup(c => c.GetEventByIdAsync(It.Is<GetEventByIdRequest>(req => req.EventId == "event2"), null, null, default))
            .Returns(GrpcTestHelpers.CreateAsyncUnaryCall(grpcReply));

        var response = await _ticketService.GetAllTicketsByUserIdAsync(userId);

        Assert.True(response.Succeeded);
        Assert.NotNull(response.Result);
        var ticket = Assert.Single(response.Result);

        Assert.Equal("book2", ticket.BookingId);
        Assert.Equal(userId, ticket.UserId);
        Assert.Equal("event2", ticket.EventId);
        Assert.Equal("Standard", ticket.TicketCategoryName);
        Assert.Equal(50, ticket.TicketPrice);
        Assert.Equal("2B", ticket.SeatNumber);
        Assert.Equal("C", ticket.Gate);

        Assert.Equal("Theatre Play", ticket.EventName);
        Assert.Equal("Drama", ticket.EventCategoryName);
        Assert.Equal("Stage", ticket.EventLocation);
        Assert.Equal(new DateOnly(2025, 6, 1), ticket.EventDate);
        Assert.Equal(new TimeOnly(19, 30), ticket.EventTime);
    }



    [Fact]
    public async Task GetTicketsByBookingIdAsync_ShouldReturnTickets_WhenTicketsExist()
    {
        var bookingId = "book3";

        var ticketModels = new List<TicketModel>
        {
            new TicketModel
            {
                Id = "3",
                BookingId = bookingId,
                UserId = "user3",
                EventId = "event3",
                TicketCategoryName = "Economy",
                TicketPrice = 30,
                SeatNumber = "3C",
                Gate = "D"
            }
        };

        var repositoryResult = new RepositoryResult<IEnumerable<TicketModel>>
        {
            Succeeded = true,
            Result = ticketModels,
            StatusCode = 200
        };

        _ticketRepository
            .Setup(r => r.GetAllAsync(
                false,
                null,
                It.IsAny<Expression<Func<TicketEntity, bool>>>(),
                0,
                Array.Empty<Expression<Func<TicketEntity, object>>>()))
            .ReturnsAsync(repositoryResult);

        var eventData = new Event
        {
            EventId = "event3",
            EventName = "Exhibition",
            EventCategoryName = "Art",
            EventLocation = "Gallery",
            EventDate = "2025-07-15",
            EventTime = "10:00",
            EventStatus = "Active",
            EventAmountOfGuests = 200
        };

        var grpcReply = new GetEventByIdReply { Event = eventData };

        _eventClient
            .Setup(c => c.GetEventByIdAsync(It.Is<GetEventByIdRequest>(req => req.EventId == "event3"), null, null, default))
            .Returns(GrpcTestHelpers.CreateAsyncUnaryCall(grpcReply));

        var response = await _ticketService.GetTicketsByBookingIdAsync(bookingId);

        Assert.True(response.Succeeded);
        Assert.NotNull(response.Result);
        var ticket = Assert.Single(response.Result);

        Assert.Equal(bookingId, ticket.BookingId);
        Assert.Equal("user3", ticket.UserId);
        Assert.Equal("event3", ticket.EventId);
        Assert.Equal("Economy", ticket.TicketCategoryName);
        Assert.Equal(30, ticket.TicketPrice);
        Assert.Equal("3C", ticket.SeatNumber);
        Assert.Equal("D", ticket.Gate);

        Assert.Equal("Exhibition", ticket.EventName);
        Assert.Equal("Art", ticket.EventCategoryName);
        Assert.Equal("Gallery", ticket.EventLocation);
        Assert.Equal(new DateOnly(2025, 7, 15), ticket.EventDate);
        Assert.Equal(new TimeOnly(10, 0), ticket.EventTime);
    }


    [Fact]
    public async Task CreateNewTicketsAsync_ShouldReturnTickets_WhenInputIsValid()
    {
        var form = new CreateTicketsForm
        {
            BookingId = "booking123",
            EventId = "event123",
            UserId = "user123",
            TicketPrice = 150,
            TicketQuantity = 2,
            TicketCategoryName = "Standard"
        };

        var generatedEntities = new List<TicketEntity>
    {
        new TicketEntity
        {
            BookingId = form.BookingId,
            EventId = form.EventId,
            UserId = form.UserId,
            TicketPrice = form.TicketPrice,
            SeatNumber = "A1",
            Gate = "G1",
            TicketCategoryName = form.TicketCategoryName
        },
        new TicketEntity
        {
            BookingId = form.BookingId,
            EventId = form.EventId,
            UserId = form.UserId,
            TicketPrice = form.TicketPrice,
            SeatNumber = "A2",
            Gate = "G1",
            TicketCategoryName = form.TicketCategoryName
        }
    };

        var returnedModels = generatedEntities.Select(e => e.MapTo<TicketModel>());

        var repositoryResponse = new TicketResponse<IEnumerable<TicketModel>>
        {
            Succeeded = true,
            StatusCode = 201,
            Result = returnedModels
        };

        _ticketRepository
            .Setup(r => r.AddRangeAsync(It.IsAny<IEnumerable<TicketEntity>>()))
            .ReturnsAsync(repositoryResponse);

        var eventInfo = new Event
        {
            EventId = form.EventId,
            EventName = "Mock Concert",
            EventCategoryName = "Music",
            EventLocation = "Test Arena",
            EventDate = "2025-12-01",
            EventTime = "19:00",
            EventStatus = "Live",
            EventAmountOfGuests = 10000
        };

        var grpcReply = new GetEventByIdReply { Event = eventInfo };

        _eventClient
            .Setup(c => c.GetEventByIdAsync(It.Is<GetEventByIdRequest>(r => r.EventId == form.EventId), null, null, default))
            .Returns(GrpcTestHelpers.CreateAsyncUnaryCall(grpcReply));

        var result = await _ticketService.CreateNewTicketsAsync(form);

        Assert.True(result.Succeeded);
        Assert.NotNull(result.Result);
        Assert.Equal(2, result.Result.Count());

        var firstTicket = result.Result.First();
        Assert.Equal(form.BookingId, firstTicket.BookingId);
        Assert.Equal(form.UserId, firstTicket.UserId);
        Assert.Equal(form.EventId, firstTicket.EventId);
        Assert.Equal(form.TicketPrice, firstTicket.TicketPrice);
        Assert.Equal(form.TicketCategoryName, firstTicket.TicketCategoryName);
    }

}
