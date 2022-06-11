namespace Web.ViewModels
{
    public class EventDetailsViewModel
    {
        public int ApiId { get; init; }
        public string Name { get; init; }
        public string Type { get; init; }
        public string Description { get; init; }
        public string Location { get; init; }
        public string? NewsUrl { get; init; }
        public string? VideoUrl { get; init; }
        public string ImageUrl { get; init; }
        public string Date { get; init; }
        public long DateJsMilliseconds { get; init; }
        public bool Upcoming { get; init; }
        public List<LaunchDetailsCardViewModel> Launches { get; init; }
        public List<SpaceProgramCardViewModel> Programs { get; init; }
    }
}
