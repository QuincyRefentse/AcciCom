namespace AcciCom.Services
{
    using AcciCom.Models;
    // Services/EventHubService.cs
    using Azure.Messaging.EventHubs;
    using Azure.Messaging.EventHubs.Producer;
    using System.Text;
    using System.Text.Json;

    public class EventHubService : IEventHubService, IAsyncDisposable
    {
        private readonly EventHubProducerClient _producerClient;
        private readonly ILogger<EventHubService> _logger;

        public EventHubService(IConfiguration configuration, ILogger<EventHubService> logger)
        {
            _logger = logger;
            // Get the connection strings from appsettings.json
            string connectionString = configuration["EventHub:ConnectionString"];
            string eventHubName = configuration["EventHub:HubName"];

            _producerClient = new EventHubProducerClient(connectionString, eventHubName);
        }

        public async Task SendAccidentReportAsync(AccidentReport report)
        {
            // Serialize the report to JSON
            string jsonReport = JsonSerializer.Serialize(report);

            // Create the event data
            EventData eventData = new EventData(Encoding.UTF8.GetBytes(jsonReport));

            // Use a using statement to create a batch
            using EventDataBatch eventBatch = await _producerClient.CreateBatchAsync();

            if (!eventBatch.TryAdd(eventData))
            {
                _logger.LogError("Event data is too large for a single batch.");
                throw new Exception("Event data is too large for a single batch.");
            }

            // Send the batch
            await _producerClient.SendAsync(eventBatch);
            _logger.LogInformation("Accident report sent to Event Hub.");
        }

        // Clean up the client when the app shuts down
        public async ValueTask DisposeAsync()
        {
            await _producerClient.DisposeAsync();
        }
    }
}
