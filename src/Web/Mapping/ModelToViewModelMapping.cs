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
                PublishDate = Utils.DateToString(article.PublishDate, true)
            };
        }

        public static LaunchCardViewModel ToLaunchCardViewModel(this Launch launch)
        {
            return new()
            { 
                ApiId = launch.ApiId,
                Name = launch.Name,
                ImageUrl = launch.ImageUrl,
                AgencyName = launch.Agency.Name,
                PadName = launch.Pad.Name,
                Status = launch.StatusName,
                Date = Utils.DateToString(launch.Date),
            };
        }

        public static LaunchViewModel ToLaunchViewModel(this Launch launch)
        {
            return new()
            {
                Name = launch.Name,
                ImageUrl = launch.ImageUrl,
                Mission = launch.Mission?.Description,
                StatusName = launch.StatusName,
                StatusDescription = launch.StatusDescription,
                Date = Utils.DateToString(launch.Date) ?? string.Empty,
                DateJsMilliseconds = launch.Date != null ? Utils.DateToJsMilliseconds(launch.Date.Value) : 0,
                Upcoming = launch.Date > DateTime.Now,
                WindowStart = Utils.DateToString(launch.WindowStart),
                WindowEnd = Utils.DateToString(launch.WindowEnd),

                Agency = launch.Agency.ToAgencyCardViewModel(),
                Rocket = launch.Rocket.ToRocketCardViewModel(),
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

        public static RocketCardViewModel ToRocketCardViewModel(this Rocket rocket)
        {
            return new()
            {
                Name = rocket.Name,
                Description = rocket.Details.Description,
                ImageUrl = rocket.Details.ImageUrl,
                InfoUrl = rocket.Details.InfoUrl,
                WikiUrl = rocket.Details.WikiUrl,
                Family = rocket.Family ?? "-",
                Variant = rocket.Variant ?? "-",
                Length = Utils.ValueToStringWithSymbol(rocket.Details.Length, "m"),
                Diameter = Utils.ValueToStringWithSymbol(rocket.Details.Diameter, "m"),
                MaxStages = Utils.ValueToStringWithSymbol(rocket.Details.MaxStages, ""),
                LaunchCost = Utils.ValueToStringWithSymbol(rocket.Details.LaunchCost, "$"),
                LiftoffMass = Utils.ValueToStringWithSymbol(rocket.Details.LiftoffMass, "T"),
                LiftoffThrust = Utils.ValueToStringWithSymbol(rocket.Details.LiftoffThrust, "kN"),
                GeoCapacity = Utils.ValueToStringWithSymbol(rocket.Details.GeoCapacity, "kg"),
                LeoCapacity = Utils.ValueToStringWithSymbol(rocket.Details.LeoCapacity, "kg"),
                SuccessfulLaunches = Utils.ValueToStringWithSymbol(rocket.Details.SuccessfulLaunches, ""),
                TotalLaunches = Utils.ValueToStringWithSymbol(rocket.Details.TotalLaunchCount, ""),
                FirstLaunch = Utils.DateToString(rocket.Details.FirstFlight) ?? "-",
                LaunchSuccessPercent = Utils.ValueToStringWithSymbol(rocket.Details.LaunchSuccessPercent, "%"),
                CostPerKgToLeo = Utils.ValueToStringWithSymbol(rocket.Details.CostPerKgToLeo, "$"),
                CostPerKgToGeo = Utils.ValueToStringWithSymbol(rocket.Details.CostPerKgToGeo, "$"),
                RankedProperties = rocket.Details.RankedProperties ?? new()
            };
        }

        public static RocketListItemViewModel ToRocketListItemViewModel(this Rocket rocket)
        {
            return new()
            { 
                ApiId = rocket.ApiId,
                Name = rocket.Name,
                Family = rocket.Family,
                Variant = rocket.Variant,
            };
        }

        public static SpaceProgramCardViewModel ToSpaceProgramCardViewModel(this SpaceProgram program)
        {
            return new()
            {
                Name = program.Name,
                Description = program.Description,
                ImageUrl = program.ImageUrl,
                StartDate = Utils.DateToString(program.StartDate, true),
                EndDate = Utils.DateToString(program.EndDate, true),
                InfoUrl = program.InfoUrl,
                WikiUrl = program.WikiUrl
            };
        }
    }
}
