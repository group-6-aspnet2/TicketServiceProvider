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

            if(DateOnly.TryParse(ticketEvent.EventDate, out var eventDate) == false)
            {
                throw new ArgumentException("Invalid event date format.", nameof(ticketEvent.EventDate));
            }

            if(TimeOnly.TryParse(ticketEvent.EventTime, out var eventTime) == false)
            {
                throw new ArgumentException("Invalid event time format.", nameof(ticketEvent.EventTime));
            }

            ticket.EventName = ticketEvent.EventName;
            ticket.EventDate = eventDate;
            ticket.EventTime = eventTime;
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
