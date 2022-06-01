using System.Text.Json.Serialization;

namespace Library.Api.Responses
{
    public record RocketConfigDetailResponse : RocketConfigResponse
    {
        [JsonPropertyName("description")]
        public string Description { get; init; }
        
        [JsonPropertyName("manufacturer")]
        public AgencyDetailResponse Manufacturer { get; init; }
        
        [JsonPropertyName("alias")]
        public string Alias { get; init; }

        [JsonPropertyName("min_stage")]
        public int? MinStage { get; init; }

        [JsonPropertyName("max_stage")]
        public int? MaxStage { get; init; }

        [JsonPropertyName("length")]
        public double? Length { get; init; }

        [JsonPropertyName("diameter")]
        public double? Diameter { get; init; }

        [JsonPropertyName("maiden_flight")]
        public DateTime? FirstFlight { get; init; }

        [JsonPropertyName("launch_cost")]
        public string? LaunchCost { get; init; }

        [JsonPropertyName("launch_mass")]
        public int? LaunchMass { get; init; }

        [JsonPropertyName("leo_capacity")]
        public int? LeoCapacity { get; init; }

        [JsonPropertyName("gto_capacity")]
        public int? GeoCapacity { get; init; }

        [JsonPropertyName("to_thrust")]
        public int? ThrustAtLiftoff { get; init; }

        [JsonPropertyName("apogee")]
        public int? Apogee { get; init; }

        [JsonPropertyName("image_url")]
        public string? ImageUrl { get; init; }

        [JsonPropertyName("wiki_url")]
        public string? WikiUrl { get; init; }

        [JsonPropertyName("total_launch_count")]
        public int TotalLaunchCount { get; init; }

        [JsonPropertyName("consecutive_successful_launches")]
        public int ConsecutiveSuccessfulLaunches { get; init; }

        [JsonPropertyName("successful_launches")]
        public int SuccessfulLaunches { get; init; }

        [JsonPropertyName("failed_launches")]
        public int FailedLaunches { get; init; }

        [JsonPropertyName("pending_launches")]
        public int PendingLaunches { get; init; }
    }
}