using Business.Services;
using Domain.Extensions;
using Domain.Models;
using Grpc.Core;
using System.Diagnostics;

namespace Presentation.GrpcServices;


public class TicketGrpcService(ITicketService ticketService) :TicketManager.TicketManagerBase
{
    private readonly ITicketService _ticketService = ticketService;

   
public override async Task<CreateTicketsReply> CreateTickets(CreateTicketsRequest request, ServerCallContext context)
{
   try
   {
       if (request == null)
           return new CreateTicketsReply
           {
               Succeeded = false,
               Message = "Request is invalid",
           };


           var serviceRequest = new CreateTicketsForm
            {
                BookingId = request.BookingId,
                UserId = request.UserId,
                EventId = request.EventId,
                TicketPrice = decimal.Parse(request.TicketPrice),
                TicketCategoryName = request.TicketCategoryName,
                TicketQuantity = request.TicketQuantity,
            };


       //     var tickets = new List<Ticket>();
       //for (int i = 0; i < request.TicketGroupQuantity; i++)
       //{
       //    tickets.Add(
       //         new Ticket
       //         {
       //             TicketId = Guid.NewGuid().ToString(),
       //             EventName = request.EventName,
       //             EventDate = request.EventDate,
       //             EventTime = request.EventTime,
       //             EventLocation = request.EventLocation,
       //             EventCategoryName = request.EventCategoryName,
       //             TicketGroupQuantity = request.TicketGroupQuantity,
       //             TicketCategoryName = request.TicketCategoryName,
       //             FirstName = request.FirstName,
       //             LastName = request.LastName,
       //             //SeatNumber = request.SeatNumber, // TODO generera typ 12A osv
       //             //Gate = request.Gate, // TODO generera bokstav A-Z
       //             BookingId = request.BookingId,
       //             InvoiceId = request.InvoiceId,
       //             EVoucherGroupId = request.EVoucherGroupId,
       //         }
       //        );
       //}

          var result = await _ticketService.CreateNewTicketsAsync(serviceRequest);

            if (!result.Succeeded)
            {
                return new CreateTicketsReply
                {
                    Succeeded = false,
                    Message = result.Error,
                };
            }

            var tickets = result.Result!.Select(x => x.MapTo<Ticket>());

       return new CreateTicketsReply
       {
           Succeeded = true,
           Message = "Tickets created successfully",
           StatusCode = result.StatusCode.ToString(),
           //Tickets = tickets
       };
   }
   catch (Exception ex)
   {
       Debug.WriteLine(ex.Message);
       return  new CreateTicketsReply
       {
           Succeeded = false,
           Message = ex.Message,
       };
   }
}


    public override Task<GetTicketsReply> GetTickets(GetTicketsRequest request, ServerCallContext context)
    {
        return base.GetTickets(request, context);
    }

    //public async override Task<GetTicketsByBookingIdReply> GetTicketsByBookingId(GetTicketsByBookingIdRequest request, ServerCallContext context)
    //{
    //    var result = await _ticketService.GetTicketsByBookingIdAsync(request.BookingId);
    //    return new GetTicketsByBookingIdReply
    //    {
    //        Tickets = { result.Result }
    //    };
    //}

    public override Task<DeleteTicketsByBookingIdReply> DeleteTicketsByBookingId(DeleteTicketsByBookingIdRequest request, ServerCallContext context)
    {
        return base.DeleteTicketsByBookingId(request, context);
    }
}
