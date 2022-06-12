using Library.Models;
using Web.ViewModels;

namespace Web.Mapping
{
    public static class ModelToViewModelMapping
    {
        public static ArticleViewModel ToArticleViewModel(this Article article)
        {
            return new()
            {
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
            };
        }

        public static LaunchDetailsViewModel ToLaunchDetailsViewModel(this Launch launch)
        {
            return new()
            {
                Launch = launch.ToLaunchDetailsCardViewModel(),
                Agency = launch.Agency.ToAgencyCardViewModel(),
                Rocket = launch.Rocket.ToRocketDetailsCardViewModel(),
                Pad = launch.Pad.ToPadCardViewModel(),
                Programs = launch.Programs.Select(p => p.ToSpaceProgramCardViewModel()).ToList(),
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
                Length = Helpers.ValueToStringWithSymbol(rocket.Details.Length, "m"),
                Diameter = Helpers.ValueToStringWithSymbol(rocket.Details.Diameter, "m"),
                MaxStages = Helpers.ValueToStringWithSymbol(rocket.Details.MaxStages, ""),
                LaunchCost = Helpers.ValueToStringWithSymbol(rocket.Details.LaunchCost, "$"),
                LiftoffMass = Helpers.ValueToStringWithSymbol(rocket.Details.LiftoffMass, "T"),
                LiftoffThrust = Helpers.ValueToStringWithSymbol(rocket.Details.LiftoffThrust, "kN"),
                GeoCapacity = Helpers.ValueToStringWithSymbol(rocket.Details.GeoCapacity, "kg"),
                LeoCapacity = Helpers.ValueToStringWithSymbol(rocket.Details.LeoCapacity, "kg"),
                SuccessfulLaunches = Helpers.ValueToStringWithSymbol(rocket.Details.SuccessfulLaunches, ""),
                TotalLaunches = Helpers.ValueToStringWithSymbol(rocket.Details.TotalLaunchCount, ""),
                FirstLaunch = Helpers.DateToString(rocket.Details.FirstFlight) ?? "-",
                LaunchSuccessPercent = Helpers.ValueToStringWithSymbol(rocket.Details.LaunchSuccessPercent, "%"),
                CostPerKgToLeo = Helpers.ValueToStringWithSymbol(rocket.Details.CostPerKgToLeo, "$"),
                CostPerKgToGeo = Helpers.ValueToStringWithSymbol(rocket.Details.CostPerKgToGeo, "$"),
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
    }
}
