using Grpc.Core;

namespace TicketServiceProvider.Services;

public class TicketGrpcService : TicketManager.TicketManagerBase
{

    public override Task<CreateTicketsReply> CreateTickets(CreateTicketsRequest request, ServerCallContext context)
    {
        return base.CreateTickets(request, context);
    }

    public override Task<GetTicketsReply> GetTickets(GetTicketsRequest request, ServerCallContext context)
    {
        return base.GetTickets(request, context);
    }

    public override Task<GetTicketsByBookingIdReply> GetTicketsByBookingId(GetTicketsByBookingIdRequest request, ServerCallContext context)
    {
        return base.GetTicketsByBookingId(request, context);
    }

    public override Task<DeleteTicketsByBookingIdReply> DeleteTicketsByBookingId(DeleteTicketsByBookingIdRequest request, ServerCallContext context)
    {
        return base.DeleteTicketsByBookingId(request, context);
    }
}
