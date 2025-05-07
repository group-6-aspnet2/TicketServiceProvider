using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class CreateTicketsForm
{
    public string BookingId { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string EventId { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal TicketPrice { get; set; }
    public int TicketQuantity { get; set; }
    public string TicketCategoryName { get; set; } = null!;
}
