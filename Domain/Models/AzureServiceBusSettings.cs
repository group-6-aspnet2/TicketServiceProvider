namespace Domain.Models;

public class AzureServiceBusSettings
{
    public string ConnectionString { get; set; } = null!;
    public string QueueName { get; set; } = null!;
}
