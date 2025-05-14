using System;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Business.Services;
using Domain.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Presentation.AzureFunctions.Functions;

public class TicketQueueService
{
    private readonly ILogger<TicketQueueService> _logger;
    private readonly ITicketService _ticketService;
    public TicketQueueService(ILogger<TicketQueueService> logger, ITicketService ticketService)
    {
        _logger = logger;
        _ticketService = ticketService;
    }

    [Function(nameof(TicketQueueService))]
    public async Task Run([ServiceBusTrigger("create-ticket", Connection = "ServiceBus")] ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions)
    {
        var body = message.Body.ToString();
        var form = JsonSerializer.Deserialize<CreateTicketsForm>(body);

        if (form != null)
        {
            var result = await _ticketService.CreateNewTicketsAsync(form);
        }

        await messageActions.CompleteMessageAsync(message);
    }
}