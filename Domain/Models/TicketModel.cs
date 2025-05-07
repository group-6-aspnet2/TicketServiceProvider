using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class TicketModel
{
    [Key]
    public string Id { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string BookingId { get; set; } = null!;
    public string EventId { get; set; } = null!;
    public string EventName { get; set; } = null!;
    public DateOnly EventDate { get; set; }
    public TimeOnly EventTime { get; set; }
    public string EventLocation { get; set; } = null!;
    public string EventCategoryName { get; set; } = null!;
    public string TicketCategoryName { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal TicketPrice { get; set; }
    public string SeatNumber { get; set; } = null!;
    public string Gate { get; set; } = null!;
    
}
