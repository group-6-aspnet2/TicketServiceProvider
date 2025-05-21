using Azure.Messaging.ServiceBus;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Business.Services;

public class CreateTicketQueueBackgroundService : BackgroundService
{

    private readonly ServiceBusProcessor _processor;
    private readonly ILogger<CreateTicketQueueBackgroundService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    public CreateTicketQueueBackgroundService(ServiceBusClient client, IConfiguration config, IServiceScopeFactory scopeFactory, ILogger<CreateTicketQueueBackgroundService> logger)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;

        _processor = client.CreateProcessor(config["AzureServiceBusSettings:CreateTicketQueueName"], new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = false
        });
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _processor.ProcessMessageAsync += HandleMessageAsync;
        _processor.ProcessErrorAsync += ErrorHandler;

        await _processor.StartProcessingAsync(stoppingToken);
    }

    private async Task HandleMessageAsync(ProcessMessageEventArgs args)
    {
        try
        {
            var body = args.Message.Body.ToString();
            var form = JsonSerializer.Deserialize<CreateTicketsForm>(body);

            using var scope = _scopeFactory.CreateScope();
            var ticketService = scope.ServiceProvider.GetRequiredService<ITicketService>();

            if (form != null)
            {
                var result = await ticketService.CreateNewTicketsAsync(form);

                if (result.Succeeded)
                {
                    await args.CompleteMessageAsync(args.Message);
                }
                else
                {
                    _logger.LogWarning("Create tickets failed: {error}", result.Error);
                    await args.AbandonMessageAsync(args.Message);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process message");
            await args.DeadLetterMessageAsync(args.Message, "ProcessingError", ex.Message);
        }
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception, "Message handler encountered an exception");
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _processor.StopProcessingAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }

}
