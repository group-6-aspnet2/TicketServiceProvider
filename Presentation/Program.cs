using Azure.Messaging.ServiceBus;
using Business.Services;
using Data.Contexts;
using Data.Interfaces;
using Data.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.Configure<AzureServiceBusSettings>(builder.Configuration.GetSection("AzureServiceBusSettings"));

builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<ITicketService, TicketService>();

builder.Services.AddTransient<IQueueService, QueueService>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var queueService = scope.ServiceProvider.GetRequiredService<IQueueService>();
    await queueService.StartAsync();
}


app.MapOpenApi();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();


//using (var scope = app.Services.CreateScope())
//{
//    var queueService = app.Services.GetRequiredService<IQueueService>();
//    await queueService.StartAsync();
//}

/*

var queueService = app.Services.GetRequiredService<IQueueService>();
await queueService.StartAsync();
builder.Services.AddHostedService<TicketListenerService>();




*/