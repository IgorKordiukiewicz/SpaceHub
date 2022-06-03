using Library.Api.Responses;

namespace Web.ViewModels
{
    public record AgencyCardViewModel
    {
        public string Name { get; init; }
        public string CountryCode { get; init; }
        public string? ImageUrl { get; init; }
        public string? Description { get; init; }

        public AgencyCardViewModel(AgencyDetailResponse agency)
        {
            Name = agency.Name;
            CountryCode = agency.CountryCode;
            ImageUrl = agency.LogoUrl;
            Description = agency.Description;
        }
    }
}
