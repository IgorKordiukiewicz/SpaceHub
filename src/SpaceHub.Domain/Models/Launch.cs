namespace SpaceHub.Domain.Models;

public class Launch
{
    public required string ApiId { get; init; }
    public required string Name { get; init; }
    public required string Status { get; set; }
    public DateTime Date { get; set; }
    public DateTime? WindowStart { get; set; }
    public DateTime? WindowEnd { get; set; }
    public required string ImageUrl { get; init; }
    public LaunchMission? Mission { get; init; }
    public required LaunchPad Pad { get; init; }
    public List<LaunchVideo> Videos { get; init; } = new();
    public required string AgencyName { get; init; }
    public required int AgencyApiId { get; init; }
    public required int RocketApiId { get; init; }
}

public class LaunchMission
{
    public required string Name { get; init; }
    public required string Type { get; init; }
    public string Description { get; init; } = string.Empty;
    public string OrbitName { get; init; } = string.Empty;
}

public class LaunchPad
{
    public required string Name { get; init; }
    public required string LocationName { get; init; }
    public required string CountryCode { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public string WikiUrl { get; init; } = string.Empty;
    public string MapImageUrl { get; init; } = string.Empty;
    public string MapUrl { get; init; } = string.Empty;
}

public class LaunchVideo
{
    public string? Title { get; init; }
    public required string Url { get; init; }
}