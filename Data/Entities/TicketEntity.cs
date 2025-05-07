using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class TicketEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string BookingId { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string EventId { get; set; } = null!;
    public string TicketCategoryName { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal TicketPrice { get; set; }
    public string SeatNumber { get; set; } = null!;
    public string Gate { get; set; } = null!;
}
