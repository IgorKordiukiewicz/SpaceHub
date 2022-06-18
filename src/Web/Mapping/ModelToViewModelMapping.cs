using Library.Enums;
using Library.Extensions;
using Library.Models;
using Web.ViewModels;

namespace Web.Mapping
{
    public static class ModelToViewModelMapping
    {
        public static ArticleCardViewModel ToArticleCardViewModel(this Article article)
        {
            return new()
            {
                ApiId = article.ApiId,
                Title = article.Title,
                Summary = article.Summary,
                Url = article.Url,
                ImageUrl = article.ImageUrl,
                NewsSite = article.NewsSite,
                PublishDate = Helpers.DateToString(article.PublishDate, true)
            };
        }

        public static LaunchIndexCardViewModel ToLaunchIndexCardViewModel(this Launch launch)
        {
            return new()
            { 
                ApiId = launch.ApiId,
                Name = launch.Name,
                ImageUrl = launch.ImageUrl,
                AgencyName = launch.Agency.Name,
                PadName = launch.Pad.Name,
                Status = launch.StatusName,
                Date = Helpers.DateToString(launch.Date),
            };
        }

        public static LaunchDetailsCardViewModel ToLaunchDetailsCardViewModel(this Launch launch)
        {
            return new()
            {
                Name = launch.Name,
                ImageUrl = launch.ImageUrl,
                Mission = launch.Mission?.Description,
                StatusName = launch.StatusName,
                StatusDescription = launch.StatusDescription,
                Date = Helpers.DateToString(launch.Date) ?? string.Empty,
                DateJsMilliseconds = launch.Date != null ? Helpers.DateToJsMilliseconds(launch.Date.Value) : 0,
                Upcoming = launch.Date > DateTime.Now,
                WindowStart = Helpers.DateToString(launch.WindowStart),
                WindowEnd = Helpers.DateToString(launch.WindowEnd),
                VideoUrl = launch.VideoUrl,
            };
        }

        public static AgencyCardViewModel ToAgencyCardViewModel(this Agency agency)
        {
            return new()
            {
                Name = agency.Name,
                CountryCode = agency.Details.CountryCode,
                ImageUrl = agency.Details.LogoUrl,
                Description = agency.Details.Description,
                InfoUrl = agency.Details.InfoUrl,
                WikiUrl = agency.Details.WikiUrl
            };
        }

        public static PadCardViewModel ToPadCardViewModel(this Pad pad)
        {
            return new()
            {
                Name = pad.Name,
                LocationName = pad.Location.Name,
                MapImageUrl = pad.MapImageUrl,
                InfoUrl = pad.InfoUrl,
                WikiUrl = pad.WikiUrl
            };
        }

        public static RocketDetailsCardViewModel ToRocketDetailsCardViewModel(this Rocket rocket)
        {
            return new()
            {
                Name = rocket.Name,
                Description = rocket.Details.Description,
                ImageUrl = rocket.ImageUrl,
                InfoUrl = rocket.InfoUrl,
                WikiUrl = rocket.WikiUrl,
                Family = rocket.Family ?? "-",
                Variant = rocket.Variant ?? "-",
                Length = Helpers.ValueToStringWithSymbol(rocket.Details.Length, RocketRankedPropertyType.Length.GetSymbol()),
                Diameter = Helpers.ValueToStringWithSymbol(rocket.Details.Diameter, RocketRankedPropertyType.Diameter.GetSymbol()),
                MaxStages = Helpers.ValueToStringWithSymbol(rocket.Details.MaxStages, ""),
                LaunchCost = Helpers.ValueToStringWithSymbol(rocket.Details.LaunchCost, RocketRankedPropertyType.LaunchCost.GetSymbol()),
                LiftoffMass = Helpers.ValueToStringWithSymbol(rocket.Details.LiftoffMass, RocketRankedPropertyType.LiftoffMass.GetSymbol()),
                LiftoffThrust = Helpers.ValueToStringWithSymbol(rocket.Details.LiftoffThrust, RocketRankedPropertyType.LiftoffThrust.GetSymbol()),
                GeoCapacity = Helpers.ValueToStringWithSymbol(rocket.Details.GeoCapacity, RocketRankedPropertyType.GeoCapacity.GetSymbol()),
                LeoCapacity = Helpers.ValueToStringWithSymbol(rocket.Details.LeoCapacity, RocketRankedPropertyType.LeoCapacity.GetSymbol()),
                SuccessfulLaunches = Helpers.ValueToStringWithSymbol(rocket.Details.SuccessfulLaunches, ""),
                TotalLaunches = Helpers.ValueToStringWithSymbol(rocket.Details.TotalLaunchCount, ""),
                FirstLaunch = Helpers.DateToString(rocket.Details.FirstFlight) ?? "-",
                LaunchSuccessPercent = Helpers.ValueToStringWithSymbol(rocket.Details.LaunchSuccessPercent, RocketRankedPropertyType.LaunchSuccessPercent.GetSymbol()),
                CostPerKgToLeo = Helpers.ValueToStringWithSymbol(rocket.Details.CostPerKgToLeo, RocketRankedPropertyType.CostPerKgToLeo.GetSymbol()),
                CostPerKgToGeo = Helpers.ValueToStringWithSymbol(rocket.Details.CostPerKgToGeo, RocketRankedPropertyType.CostPerKgToGeo.GetSymbol()),
                RankedProperties = rocket.Details.RankedProperties ?? new()
            };
        }

        public static RocketIndexCardViewModel ToRocketIndexCardViewModel(this Rocket rocket)
        {
            return new()
            { 
                ApiId = rocket.ApiId,
                Name = rocket.Name,
                Family = rocket.Family,
                Variant = rocket.Variant,
                ImageUrl = rocket.ImageUrl
            };
        }

        public static SpaceProgramCardViewModel ToSpaceProgramCardViewModel(this SpaceProgram program)
        {
            return new()
            {
                Name = program.Name,
                Description = program.Description,
                ImageUrl = program.ImageUrl,
                StartDate = Helpers.DateToString(program.StartDate, true),
                EndDate = Helpers.DateToString(program.EndDate, true),
                InfoUrl = program.InfoUrl,
                WikiUrl = program.WikiUrl
            };
        }

        public static EventCardViewModel ToEventCardViewModel(this Event event_)
        {
            return new()
            {
                ApiId = event_.ApiId,
                Name = event_.Name,
                Type = event_.Type,
                Description = event_.Description,
                Location = event_.Location ?? "-",
                ImageUrl = event_.ImageUrl,
                NewsUrl = event_.NewsUrl,
                VideoUrl = event_.VideoUrl,
                Date = Helpers.DateToString(event_.Date) ?? "-"
            };
        }

        public static EventDetailsViewModel ToEventDetailsViewModel(this Event event_)
        {
            return new()
            {
                ApiId = event_.ApiId,
                Name = event_.Name,
                Type = event_.Type,
                Description = event_.Description,
                Location = event_.Location ?? "-",
                ImageUrl = event_.ImageUrl,
                NewsUrl = event_.NewsUrl,
                VideoUrl = event_.VideoUrl,
                Date = Helpers.DateToString(event_.Date) ?? "-",
                DateJsMilliseconds = event_.Date != null ? Helpers.DateToJsMilliseconds(event_.Date.Value) : 0,
                Upcoming = event_.Date > DateTime.Now,
                Launch = event_.Launches.Count > 0 ? event_.Launches.First().ToLaunchDetailsCardViewModel() : null,
                Programs = event_.Programs.Select(p => p.ToSpaceProgramCardViewModel()).ToList(),
            };
        }

        public static RocketRankedPropertyViewModel ToRocketRankedPropertyViewModel(this RocketRankedProperty rocketRankedProperty)
        {
            return new()
            {
                ApiId = rocketRankedProperty.Rocket.ApiId,
                RocketName = rocketRankedProperty.Rocket.FullName,
                Value = Helpers.ValueToStringWithSymbol(rocketRankedProperty.Value, rocketRankedProperty.Type.GetSymbol()),
                SecondaryValue = rocketRankedProperty.SecondaryValue?.ToString()
            };
        }
    }
}
