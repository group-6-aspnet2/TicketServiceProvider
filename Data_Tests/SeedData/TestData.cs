using Data.Entities;

namespace Data_Tests.SeedData;

public static class TestData
{
    public static readonly TicketEntity[] TicketEntities = [
        new TicketEntity{
            Id = "id-123-id",
            BookingId = "booking-id-987",
            UserId ="user-id-123",
            EventId= "event-id-678",
            TicketCategoryName = "VIP",
            TicketPrice = 1099,
            SeatNumber = "A1",
            Gate = "J"
        },
             new TicketEntity{
            Id = "id-543241-id",
            BookingId = "booking-id-987",
            UserId ="user-id-123",
            EventId= "event-id-678",
            TicketCategoryName = "VIP",
            TicketPrice = 1099,
            SeatNumber = "A2",
            Gate = "J"
        },

        new TicketEntity{
            Id = "id-456-id",
            BookingId = "booking-id-654",
            UserId ="user-id-456",
            EventId= "event-id-321",
            TicketCategoryName = "Standard",
            TicketPrice = 300,
            SeatNumber = "E59",
            Gate = "D"
        },
        ];
}
