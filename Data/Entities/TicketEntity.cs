using System;

namespace Data.Entities;

public class TicketEntity
{
    public string Id { get; set; } = null!;
    public string EventName { get; set; } = null!;
    public DateOnly EventDate { get; set; }
    public TimeOnly EventTime { get; set; }
    public string EventLocation { get; set; } = null!;
    public string EventCategoryName { get; set; } = null!;
    public string TicketCategoryName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string SeatNumber { get; set; } = null!;
    public string Gate { get; set; } = null!;
    public string BookingId { get; set; } = null!;
    public string InvoiceId { get; set; } = null!;
}
