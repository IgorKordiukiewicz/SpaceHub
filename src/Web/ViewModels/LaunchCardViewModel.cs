using Library.Api.Responses;
using System.Globalization;

namespace Web.ViewModels
{
    public record LaunchCardViewModel
    {
        public string ApiId { get; init; }
        public string Name { get; init; }
        public string ImageUrl { get; init; }
        public string AgencyName { get; init; }
        public string PadName { get; init; }
        public string Status { get; init; }
        public string? Date { get; init; }
    }
}
