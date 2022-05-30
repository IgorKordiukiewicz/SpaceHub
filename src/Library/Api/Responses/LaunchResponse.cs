using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Library.Api.Responses
{
    public record LaunchResponse
    {
        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("status")]
        public LaunchStatusResponse Status { get; init; }

        [JsonPropertyName("net")]
        public DateTime? Date { get; init; }

        [JsonPropertyName("window_start")]
        public DateTime? WindowStart { get; init; }

        [JsonPropertyName("window_end")]
        public DateTime? WindowEnd { get; init; }

        [JsonPropertyName("launch_service_provider")]
        public LaunchServiceProviderResponse ServiceProvider { get; init; }

        [JsonPropertyName("rocket")]
        public LaunchRocketResponse Rocket { get; init; }

        [JsonPropertyName("mission")]
        public LaunchMissionResponse Mission { get; init; }

        [JsonPropertyName("pad")]
        public LaunchPadResponse Pad { get; init; }

        [JsonPropertyName("program")]
        public List<LaunchProgramResponse> Programs { get; init; }

        [JsonPropertyName("image")]
        public string ImageUrl { get; init; }
    }
}
