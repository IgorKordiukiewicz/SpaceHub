namespace Web.ViewModels
{
    public record RocketRankedPropertyViewModel
    {
        public int ApiId { get; init; }
        public string RocketName { get; init; }
        public string Value { get; init; }
        public string? SecondaryValue { get; init; }
    }
}
