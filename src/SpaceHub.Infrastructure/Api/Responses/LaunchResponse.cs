﻿using System.Text.Json.Serialization;

namespace SpaceHub.Infrastructure.Api.Responses;
public record LaunchResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }
    
    [JsonPropertyName("name")]
    public string Name { get; init; }

    [JsonPropertyName("status")]
    public LaunchStatusResponse Status { get; init; }

    [JsonPropertyName("net")]
    public DateTime Date { get; init; }

    [JsonPropertyName("window_start")]
    public DateTime? WindowStart { get; init; }

    [JsonPropertyName("window_end")]
    public DateTime? WindowEnd { get; init; }

    [JsonPropertyName("launch_service_provider")]
    public AgencyResponse Agency { get; init; }

    [JsonPropertyName("rocket")]
    public RocketResponse Rocket { get; init; }

    [JsonPropertyName("mission")]
    public MissionResponse? Mission { get; init; }

    [JsonPropertyName("pad")]
    public PadResponse Pad { get; init; }

    [JsonPropertyName("program")]
    public IReadOnlyList<ProgramResponse> Programs { get; init; }

    [JsonPropertyName("image")]
    public string ImageUrl { get; init; }
}
