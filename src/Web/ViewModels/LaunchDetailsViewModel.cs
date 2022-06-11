using Library.Api.Responses;
using System.Globalization;

namespace Web.ViewModels
{
    public record LaunchDetailsViewModel
    {
        public LaunchDetailsCardViewModel Launch { get; init; }
        public AgencyCardViewModel Agency { get; init; }
        public RocketDetailsCardViewModel Rocket { get; init; }
        public PadCardViewModel Pad { get; init; }
        public List<SpaceProgramCardViewModel> Programs { get; init; }
    }
}
