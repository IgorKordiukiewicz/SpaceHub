using Library.Api.Responses;
using System.Globalization;

namespace Web.ViewModels
{
    public record LaunchViewModel
    {
        public string Name { get; init; }
        public string ImageUrl { get; init; }    
        public string? Mission { get; init; }
        public string Date { get; init; }
        public long DateJsMilliseconds { get; init; }
        public bool Upcoming { get; init; }
        public string? WindowStart { get; init; }
        public string? WindowEnd { get; init; }
        public string StatusName { get; init; }
        public string StatusDescription { get; init; }
        public AgencyCardViewModel Agency { get; init; }
        public RocketDetailsCardViewModel Rocket { get; init; }
        public PadCardViewModel Pad { get; init; }
        public List<SpaceProgramCardViewModel> Programs { get; init; }
    }
}
