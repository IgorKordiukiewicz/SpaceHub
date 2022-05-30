using System.Text.Json.Serialization;

namespace Library.Api.Responses
{
    public record AgencyDetailResponse : AgencyResponse
    {
        [JsonPropertyName("country_code")]
        public string CountryCode { get; init; }

        [JsonPropertyName("abbrev")]
        public string Abbrevation { get; init; }

        [JsonPropertyName("description")]
        public string? Description { get; init; }

        [JsonPropertyName("administrator")]
        public string? Administrator { get; init; }

        [JsonPropertyName("founding_year")]
        public string? FoundingYear { get; init; }

        [JsonPropertyName("launchers")]
        public string Launchers { get; init; }

        [JsonPropertyName("spacecraft")]
        public string Spacecraft { get; init; }

        [JsonPropertyName("launch_library_url")]
        public string LaunchLibraryUrl { get; init; }

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

        [JsonPropertyName("consecutive_successful_landings")]
        public int ConsecutiveSuccessfulLandings { get; init; }

        [JsonPropertyName("successful_landings")]
        public int SuccessfulLandings { get; init; }

        [JsonPropertyName("failed_landings")]
        public int FailedLandings { get; init; }

        [JsonPropertyName("attempted_landings")]
        public int AttemptedLandings { get; init; }

        [JsonPropertyName("info_url")]
        public string? InfoUrl { get; init; }

        [JsonPropertyName("wiki_url")]
        public string? WikiUrl { get; init; }

        [JsonPropertyName("logo_url")]
        public string? LogoUrl { get; init; }

        [JsonPropertyName("image_url")]
        public string? ImageUrl { get; init; }

        [JsonPropertyName("nation_url")]
        public string? NationUrl { get; init; }
    }
}