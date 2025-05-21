using Domain.Models;
using System.Diagnostics;

namespace Business.Factories;

public static class TicketFactory
{
    public static TicketModel? MapEventToTicketModel(TicketModel ticket, Event ticketEvent)
    {
		try
		{
            ArgumentNullException.ThrowIfNull(ticket);
            ArgumentNullException.ThrowIfNull(ticketEvent);


            ticket.EventName = ticketEvent.EventName;
            ticket.EventDate = DateOnly.Parse(ticketEvent.EventDate);
            ticket.EventTime = TimeOnly.Parse(ticketEvent.EventTime);
            ticket.EventCategoryName = ticketEvent.EventCategoryName;
            ticket.EventLocation = ticketEvent.EventLocation;

            return ticket;
        }
        catch (Exception ex)
		{
            Debug.WriteLine(ex.Message);
            return null;
        }

    }
}
