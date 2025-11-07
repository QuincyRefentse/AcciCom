namespace AcciCom.Models
{
    // Models/AccidentReport.cs
    using System.Text.Json.Serialization;

    public class AccidentReport
    {
        // We use JsonPropertyName to ensure the JSON is clean
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString(); // For Cosmos DB

        [JsonPropertyName("vehicleId")]
        public string VehicleId { get; set; } // e.g., "GP-123-ABC" (from QR)

        [JsonPropertyName("driverName")]
        public string DriverName { get; set; } // (from QR)

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; } // (from device)

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; } // (from device)

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("status")]
        public string Status { get; set; } = "Reported";
    }
}
