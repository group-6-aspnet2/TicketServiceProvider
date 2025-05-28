using Business;
using Business.Factories;
using Domain.Models;

namespace Business_Tests.Factories;

public class TicketFactory_Tests
{

      [Fact]
    public void MapEventToTicketModel_ShouldReturnTicketModelWithMappedEvent_WhenValidInput()
    {
        var ticket = new TicketModel
        {
            Id = "1",
            UserId = "user123",
            BookingId = "booking123",
            EventId = "event123",
            TicketCategoryName = "VIP",
            TicketPrice = 1000,
            SeatNumber = "A1",
            Gate = "North"
        };

        var ticketEvent = new Event
        {
            EventId = "event123",
            EventName = "Tech Conference",
            EventCategoryName = "Technology",
            EventLocation = "Gothenburg",
            EventDate = "2025-09-10",
            EventTime = "10:00",
            EventStatus = "Active",
            EventAmountOfGuests = 500
        };

        var result = TicketFactory.MapEventToTicketModel(ticket, ticketEvent);

        Assert.NotNull(result);
        Assert.Equal("Tech Conference", result.EventName);
        Assert.Equal(new DateOnly(2025, 9, 10), result.EventDate);
        Assert.Equal(new TimeOnly(10, 0), result.EventTime);
        Assert.Equal("Technology", result.EventCategoryName);
        Assert.Equal("Gothenburg", result.EventLocation);
        Assert.Equal("VIP", result.TicketCategoryName);
        Assert.Equal(1000, result.TicketPrice);
        Assert.Equal("A1", result.SeatNumber);
        Assert.Equal("North", result.Gate);
    }

    [Fact]
    public void MapEventToTicketModel_ShouldReturnNull_WhenTicketIsNull()
    {
        var ticketEvent = new Event
        {
            EventName = "Concert",
            EventDate = "2025-06-01",
            EventTime = "19:30",
            EventCategoryName = "Music",
            EventLocation = "Stockholm"
        };

        var result = TicketFactory.MapEventToTicketModel(null!, ticketEvent);
        Assert.Null(result);
    }

    [Fact]
    public void MapEventToTicketModel_ShouldReturnNull_WhenEventIsNull()
    {
        var ticket = new TicketModel();

        var result = TicketFactory.MapEventToTicketModel(ticket, null!);

        Assert.Null(result);
    }

    [Fact]
    public void MapEventToTicketModel_ShouldReturnNull_WhenEventDateIsInvalid()
    {
        var ticket = new TicketModel();
        var ticketEvent = new Event
        {
            EventName = "Concert",
            EventDate = "not-a-date",
            EventTime = "19:30",
            EventCategoryName = "Music",
            EventLocation = "Stockholm"
        };

        var result = TicketFactory.MapEventToTicketModel(ticket, ticketEvent);

        Assert.Null(result);
    }

    [Fact]
    public void MapEventToTicketModel_ShouldReturnNull_WhenEventTimeIsInvalid()
    {
        var ticket = new TicketModel();
        var ticketEvent = new Event
        {
            EventName = "Concert",
            EventDate = "2025-06-01",
            EventTime = "not-a-time",
            EventCategoryName = "Music",
            EventLocation = "Stockholm"
        };

        var result = TicketFactory.MapEventToTicketModel(ticket, ticketEvent);

        Assert.Null(result);
    }

}
