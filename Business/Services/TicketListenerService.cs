using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Business.Services;

public class TicketListenerService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<TicketListenerService> _logger;

    public TicketListenerService(IServiceScopeFactory scopeFactory, ILogger<TicketListenerService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("TicketListenerService started.");

        using (var scope = _scopeFactory.CreateScope())
        {
            var ticketService = scope.ServiceProvider.GetRequiredService<ITicketService>();
            await ticketService.ListenAsync(); 
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("TicketListenerService stopped.");
        return Task.CompletedTask;
    }
}

