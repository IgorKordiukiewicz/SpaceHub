using Library.Api.Responses;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Mapping
{
    public static class ResponseToModelMapping
    {
        public static Article ToModel(this ArticleResponse response)
        {
            return new()
            {
                ApiId = response.Id,
                Title = response.Title,
                Summary = response.Summary,
                Url = response.Url,
                ImageUrl = response.ImageUrl,
                NewsSite = response.NewsSite,
                PublishDate = response.PublishDate,
                UpdateDate = response.UpdateDate,
            };
        }

        public static Launch ToModel(this LaunchResponse response)
        {
            return new()
            {
                ApiId = response.Id,
                Name = response.Name,
                StatusName = response.Status.Name,
                StatusDescription = response.Status.Description,
                StatusAbbrevation = response.Status.Abbrevation,
                Date = response.Date,
                WindowStart = response.WindowStart,
                WindowEnd = response.WindowEnd,
                Agency = response.Agency.ToModel(),
                Rocket = response.Rocket.Configuration.ToModel(),
                Pad = response.Pad.ToModel(),
                Mission = response.Mission?.ToModel(),
                Programs = response.Programs.Select(p => p.ToModel()).ToList(),
                ImageUrl = response.ImageUrl,
            };
        }

        public static Launch ToModel(this LaunchDetailResponse response)
        {
            return new()
            {
                ApiId = response.Id,
                Name = response.Name,
                StatusName = response.Status.Name,
                StatusDescription = response.Status.Description,
                StatusAbbrevation = response.Status.Abbrevation,
                Date = response.Date,
                WindowStart = response.WindowStart,
                WindowEnd = response.WindowEnd,
                Agency = response.Agency.ToModel(),
                Rocket = response.Rocket.Configuration.ToModel(),
                Pad = response.Pad.ToModel(),
                Mission = response.Mission?.ToModel(),
                Programs = response.Programs.Select(p => p.ToModel()).ToList(),
                ImageUrl = response.ImageUrl,
                VideoUrl = response.Videos?.FirstOrDefault()?.Url
            };
        }

        public static Agency ToModel(this AgencyResponse response)
        {
            return AgencyResponseToModel(response);
        }

        public static Agency ToModel(this AgencyDetailResponse response)
        {
            var result = AgencyResponseToModel(response);
            result.Details = new()
            {
                CountryCode = response.CountryCode,
                Abbrevation = response.Abbrevation,
                Description = response.Description,
                Administrator = response.Administrator,
                FoundingYear = response.FoundingYear,
                Launchers = response.Launchers,
                Spacecraft = response.Spacecraft,
                LaunchLibraryUrl = response.LaunchLibraryUrl,
                TotalLaunchCount = response.TotalLaunchCount,
                ConsecutiveSuccessfulLaunches = response.ConsecutiveSuccessfulLaunches,
                SuccessfulLaunches = response.SuccessfulLaunches,
                FailedLaunches = response.FailedLaunches,
                PendingLaunches = response.PendingLaunches,
                ConsecutiveSuccessfulLandings = response.ConsecutiveSuccessfulLandings,
                SuccessfulLandings = response.SuccessfulLandings,
                FailedLandings = response.FailedLandings,
                AttemptedLandings = response.AttemptedLandings,
                InfoUrl = response.InfoUrl,
                WikiUrl = response.WikiUrl,
                LogoUrl = response.LogoUrl,
                ImageUrl = response.ImageUrl,
                NationUrl = response.NationUrl
            };

            return result;
        }

        public static Rocket ToModel(this RocketConfigResponse response)
        {
            return RocketResponseToModel(response);
        }

        public static Rocket ToModel(this RocketConfigDetailResponse response)
        {
            var result = RocketResponseToModel(response);
            result.Details = new()
            {
                Description = response.Description,
                Manufacturer = response.Manufacturer.ToModel(),
                Programs = response.Programs.Select(p => p.ToModel()).ToList(),
                Alias = response.Alias,
                MinStages = response.MinStage,
                MaxStages = response.MaxStage,
                Length = response.Length,
                Diameter = response.Diameter,
                FirstFlight = response.FirstFlight,
                LaunchCost = Int32.TryParse(response.LaunchCost, out int launchCostResult) ? launchCostResult : null, 
                LeoCapacity = response.LeoCapacity,
                GeoCapacity = response.GeoCapacity,
                LiftoffMass = response.LaunchMass,
                LiftoffThrust = response.ThrustAtLiftoff,
                Apogee = response.Apogee,
                TotalLaunchCount = response.TotalLaunchCount,
                ConsecutiveSuccessfulLaunches = response.ConsecutiveSuccessfulLaunches,
                SuccessfulLaunches = response.SuccessfulLaunches,
                FailedLaunches = response.FailedLaunches,
                PendingLaunches = response.PendingLaunches
            };

            return result;
        }

        public static Mission ToModel(this MissionResponse response)
        {
            return new()
            {
                Name = response.Name,
                Description = response.Description,
                Designator = response.Designator,
                Type = response.Type,
                OrbitName = response.Orbit?.Name,
                OrbitAbbrevation = response.Orbit?.Abbrevation
            };
        }

        public static Pad ToModel(this PadResponse response)
        {
            return new()
            {
                AgencyId = response.AgencyId,
                Name = response.Name,
                InfoUrl = response.InfoUrl,
                WikiUrl = response.WikiUrl,
                MapUrl = response.MapUrl,
                Latitude = response.Latitude,
                Longitude = response.Longitude,
                Location = response.Location.ToModel(),
                MapImageUrl = response.MapImageUrl,
                TotalLaunches = response.TotalLaunches
            };
        }

        public static PadLocation ToModel(this PadLocationResponse response)
        {
            return new()
            {
                Name = response.Name,
                CountryCode = response.CountryCode,
                MapImageUrl = response.MapImageUrl,
                TotalLaunches = response.TotalLaunches,
                TotalLandings = response.TotalLandings
            };
        }

        public static SpaceProgram ToModel(this ProgramResponse response)
        {
            return new()
            {
                Name = response.Name,
                Description = response.Description,
                Agencies = response.Agencies.Select(a => a.ToModel()).ToList(),
                ImageUrl = response.ImageUrl,
                StartDate = response.StartDate,
                EndDate = response.EndDate,
                InfoUrl= response.InfoUrl,
                WikiUrl = response.WikiUrl
            };
        }

        public static Event ToModel(this EventResponse response)
        {
            return new()
            {
                ApiId = response.Id,
                Name = response.Name,
                Type = response.Type.Name,
                Description = response.Description,
                Location = response.Location,
                NewsUrl = response.NewsUrl,
                VideoUrl = response.VideoUrl,
                ImageUrl = response.ImageUrl,
                Date = response.Date,
                Launches = response.Launches.Select(l => l.ToModel()).ToList(),
                Programs = response.Programs.Select(p => p.ToModel()).ToList()
            };
        }

        private static Agency AgencyResponseToModel(AgencyResponse response)
        {
            return new()
            {
                Name = response.Name,
                Type = response.Type,
            };
        }

        private static Rocket RocketResponseToModel(RocketConfigResponse response)
        {
            return new()
            {
                ApiId = response.Id,
                Name = response.Name,
                Family = response.Family,
                FullName = response.FullName,
                Variant = response.Variant,
                ImageUrl = response.ImageUrl,
                InfoUrl = response.InfoUrl,
                WikiUrl = response.WikiUrl
            };
        }
    }
}
