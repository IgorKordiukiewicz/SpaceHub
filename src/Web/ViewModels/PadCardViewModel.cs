using Library.Api.Responses;

namespace Web.ViewModels
{
    public record PadCardViewModel
    {
        public string Name { get; init; }
        public string LocationName { get; init; }
        public string MapImageUrl { get; init; }

        public PadCardViewModel(PadResponse pad)
        {
            Name = pad.Name;
            LocationName = pad.Location.Name;
            MapImageUrl = pad.MapImageUrl;
        }
    }
}
