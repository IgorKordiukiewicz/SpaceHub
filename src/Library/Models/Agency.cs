namespace Library.Models;

public class Agency
{
    public string Name { get; init; }
    public string Type { get; init; }
    public record Detail
    {
        public string CountryCode { get; init; }
        public string Abbrevation { get; init; }
        public string? Description { get; init; }
        public string? Administrator { get; init; }
        public string? FoundingYear { get; init; }
        public string Launchers { get; init; }
        public string Spacecraft { get; init; }
        public string LaunchLibraryUrl { get; init; }
        public int TotalLaunchCount { get; init; }
        public int ConsecutiveSuccessfulLaunches { get; init; }
        public int SuccessfulLaunches { get; init; }
        public int FailedLaunches { get; init; }
        public int PendingLaunches { get; init; }
        public int ConsecutiveSuccessfulLandings { get; init; }
        public int SuccessfulLandings { get; init; }
        public int FailedLandings { get; init; }
        public int AttemptedLandings { get; init; }
        public string? InfoUrl { get; init; }
        public string? WikiUrl { get; init; }
        public string? LogoUrl { get; init; }
        public string? ImageUrl { get; init; }
        public string? NationUrl { get; init; }
    };
    public Detail? Details { get; set; }
}
