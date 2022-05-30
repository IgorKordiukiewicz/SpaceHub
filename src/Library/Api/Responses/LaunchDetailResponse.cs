using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Library.Api.Responses
{
    public record LaunchDetailResponse : LaunchResponse
    {
        [JsonPropertyName("r_spacex_api_id")]
        public string? SpaceXApiId { get; init; }

        [JsonPropertyName("launch_service_provider")]
        public new AgencyDetailResponse ServiceProvider { get; init; }

        [JsonPropertyName("rocket")]
        public new RocketDetailResponse Rocket { get; init; }

        [JsonPropertyName("orbital_launch_attempt_count")]
        public int OrbitalLaunchAttemptCount { get; init; }

        [JsonPropertyName("location_launch_attempt_count")]
        public int LocationLaunchAttemptCount { get; init; }

        [JsonPropertyName("pad_launch_attempt_count")]
        public int PadLaunchAttemptCount { get; init; }

        [JsonPropertyName("agency_launch_attempt_count")]
        public int AgencyLaunchAttemptCount { get; init; }

        [JsonPropertyName("orbital_launch_attempt_count_year")]
        public int OrbitalLaunchAttemptCountYear { get; init; }

        [JsonPropertyName("location_launch_attempt_count_year")]
        public int LocationLaunchAttemptCountYear { get; init; }

        [JsonPropertyName("pad_launch_attempt_count_year")]
        public int PadLaunchAttemptCountYear { get; init; }

        [JsonPropertyName("agency_launch_attempt_count_year")]
        public int AgencyLaunchAttemptCountYear { get; init; }
    }
}
