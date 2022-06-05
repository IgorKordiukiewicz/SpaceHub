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
                Alias = response.Alias,
                MinStage = response.MinStage,
                MaxStage = response.MaxStage,
                Length = response.Length,
                Diameter = response.Diameter,
                FirstFlight = response.FirstFlight,
                LaunchCost = response.LaunchCost != null ? Int32.Parse(response.LaunchCost) : null,
                LeoCapacity = response.LeoCapacity,
                GeoCapacity = response.GeoCapacity,
                LaunchMass = response.LaunchMass,
                ThrustAtLiftoff = response.ThrustAtLiftoff,
                Apogee = response.Apogee,
                ImageUrl = response.ImageUrl,
                WikiUrl = response.WikiUrl,
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
                OrbitName = response.Orbit.Name,
                OrbitAbbrevation = response.Orbit.Abbrevation
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

        public static Program ToModel(this ProgramResponse response)
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
            };
        }
    }
}
