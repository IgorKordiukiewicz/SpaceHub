namespace Web.ViewModels
{
    public record EventIndexCardViewModel
    {
        public int ApiId { get; init; }
        public string Name { get; init; }
        public string Type { get; init; }
        public string Description { get; init; }
        public string Location { get; init; }
        public string ImageUrl { get; init; }
        public string Date { get; init; }
    }
}
