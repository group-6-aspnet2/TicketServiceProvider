using Azure.Messaging.ServiceBus;
using Business;
using Business.Services;
using Data.Contexts;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v.1.0",
        Title = "TicketService API Documentation",
        Description = "Official documentation for Ticket Service Provider API."

    });

    o.EnableAnnotations();
    o.ExampleFilters();
});
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<ITicketService, TicketService>();

builder.Services.AddSingleton<ServiceBusClient>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new ServiceBusClient(configuration["AzureServiceBusSettings:ConnectionString"]);
});

builder.Services.AddHostedService<CreateTicketQueueBackgroundService>(); // listener 

builder.Services.AddGrpcClient<EventContract.EventContractClient>(x =>
{
    x.Address = new Uri(builder.Configuration["GrpcClients:EventService"]!);
});



var app = builder.Build();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.MapOpenApi();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ticket Service API - v.1.0");
    c.RoutePrefix = string.Empty; 
});
app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthorization();
app.MapControllers();
app.Run();
