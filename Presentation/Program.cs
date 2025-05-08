using Azure.Messaging.ServiceBus;
using Business.Services;
using Data.Contexts;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddSingleton<ServiceBusClient>( provider =>
{
    var serviceBusConnectionString = provider.GetRequiredService<IConfiguration>()["ServiceBus:ConnectionString"];
    return new ServiceBusClient(serviceBusConnectionString);
});
builder.Services.AddHostedService<TicketListenerService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var ticketService = scope.ServiceProvider.GetRequiredService<ITicketService>();
    await ticketService.ListenAsync(); 
}

app.MapOpenApi();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
