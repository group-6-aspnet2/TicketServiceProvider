//using Azure.Messaging.ServiceBus;
//using Data.Interfaces;
//using Domain.Models;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Options;
//using System.Text.Json;

//namespace Business.Services;

//public interface IQueueService
//{
//    Task StartAsync();
//    Task StopAsync();
//}

//public class QueueService  : /* BackgroundService,*/  IQueueService
//{
//    private readonly ITicketService _ticketService;
//    private readonly AzureServiceBusSettings _settings;
//    private readonly ServiceBusClient _client;
//    private ServiceBusProcessor _processor;
//    //private readonly IServiceScopeFactory _scopeFactory;

//    public QueueService(IOptions<AzureServiceBusSettings> options, ITicketService ticketService/*, IServiceScopeFactory scopeFactory*/)
//    {
//        _settings = options.Value;
//        _client = new ServiceBusClient(_settings.ConnectionString);
//        _processor = _client.CreateProcessor(_settings.QueueName, new ServiceBusProcessorOptions());
//        //_scopeFactory = scopeFactory;

        
//        _ticketService = ticketService;
//        RegisterMessageHandler();
//        RegisterErrorHandler();
//    }

//    //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    //{
//    //    while (!stoppingToken.IsCancellationRequested)
//    //    {
//    //        using var scope = _scopeFactory.CreateScope();
//    //        var ticketService = scope.ServiceProvider.GetRequiredService<ITicketService>();

//    //        await StartAsync();

//    //    }
//    //}

//    private void RegisterMessageHandler()
//    {
//        _processor.ProcessMessageAsync += async args =>
//        {
//            try
//            {
//                var body = args.Message.Body.ToString();
//                var form = JsonSerializer.Deserialize<CreateTicketsForm>(body) ?? throw new Exception("Failed to deserialize message body.");

//                Console.WriteLine("Form i string: ", form!.ToString());
//                var result = await _ticketService.CreateNewTicketsAsync(form);

//                if (result.Succeeded)
//                    await args.CompleteMessageAsync(args.Message);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Message handling failed: {ex.Message}");
//                await args.AbandonMessageAsync(args.Message);
//            }
//        };
//    }

//    private void RegisterErrorHandler()
//    {
//        _processor.ProcessErrorAsync += args =>
//        {
//            Console.WriteLine($"Error processing message: {args.Exception.Message}");
//            return Task.CompletedTask;
//        };
//    }

//    public async Task StartAsync()
//    {
//        await _processor.StartProcessingAsync();
//    }

//    public async Task StopAsync()
//    {
//        await _processor.StopProcessingAsync();
//    }

//}
