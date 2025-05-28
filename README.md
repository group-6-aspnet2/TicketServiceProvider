# TicketServiceProvider
TicketServiceProvider is a microservice built with ASP.NET Core that handles ticket generation and retrieval for a booking system. It exposes a REST API for querying tickets and includes a background service that listens to an Azure Service Bus queue to automatically create tickets in response to booking events.


## Technologies used
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Azure Service Bus Queues
- Background Services (HostedService)
- Dependency Injection
- Clean architecture principles
- Solid
- Dynamic mapping
- Factories

### Azure Service Bus Integration 
This service listens to a Service Bus Queue (e.g., create-tickets) using a background service. Workflow: Another microservice (e.g., BookingServiceProvider) sends a message to the create-tickets queue after a booking is made. The background service picks up the message and extracts the ticket information. Tickets are created and saved to the database accordingly.
