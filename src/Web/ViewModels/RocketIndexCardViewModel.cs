namespace Web.ViewModels;

public record RocketIndexCardViewModel
{
    public int ApiId { get; init; }
    public string Name { get; init; }
    public string Family { get; init; }
    public string Variant { get; init; }
    public string ImageUrl { get; init; }
}
