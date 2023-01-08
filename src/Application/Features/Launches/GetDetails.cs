using MediatR;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using SpaceHub.Contracts.ViewModels;
using SpaceHub.Domain;
using SpaceHub.Domain.Enums;
using SpaceHub.Domain.Extensions;
using SpaceHub.Infrastructure.Api;
using SpaceHub.Infrastructure.Data;
using MongoDB.Driver.Linq;
using SpaceHub.Domain.Models;

namespace SpaceHub.Application.Features.Launches;

public record GetLaunchDetailsQuery(string Id) : IRequest<LaunchDetailsVM>;

internal class GetLaunchDetailsHandler : IRequestHandler<GetLaunchDetailsQuery, LaunchDetailsVM>
{
    private readonly DbContext _db;

    public GetLaunchDetailsHandler(DbContext db)
    {
        _db = db;
    }

    public async Task<LaunchDetailsVM> Handle(GetLaunchDetailsQuery request, CancellationToken cancellationToken)
    {
        var launch = await _db.Launches.AsQueryable().FirstOrDefaultAsync(x => x.ApiId == request.Id);
        if (launch is null)
        {
            // TODO
        }
        
        var agency = await _db.Agencies.AsQueryable().FirstOrDefaultAsync(x => x.ApiId == launch.AgencyApiId);
        if (agency is null)
        {
            // TODO
        }
        
        var rocket = await _db.Rockets.AsQueryable()
            .Where(x => x.ApiId == launch.RocketApiId)
            .Select(x => new Rocket()
            {
                ApiId = x.ApiId,
                Name= x.Name,
                Family = x.Family,
                Variant = x.Variant,
                Active = x.Active,
                Reusable = x.Reusable,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                WikiUrl = x.WikiUrl,
                InfoUrl = x.InfoUrl,
                Length = x.Length,
                Diameter = x.Diameter,
                MaxStages = x.MaxStages,
                LaunchCost = x.LaunchCost,
                LiftoffMass = x.LiftoffMass,
                ThrustAtLiftoff = x.ThrustAtLiftoff,
                LeoCapacity = x.LeoCapacity,
                GeoCapacity = x.GeoCapacity,
                SuccessfulLaunches = x.SuccessfulLaunches,
                TotalLaunches = x.TotalLaunches,
                ConsecutiveSuccessfulLaunches = x.ConsecutiveSuccessfulLaunches,
                PendingLaunches = x.PendingLaunches,
                FirstFlight = x.FirstFlight
            }).FirstOrDefaultAsync();
        if (rocket is null)
        {
            // TODO
        }
        
        static string PropertyAsString<T>(T? value)
        {
            return value is null ? "-" : value.ToString();
        }
        
        // TODO
        var properties = new List<RocketPropertyVM>()
        {
            new(ERocketProperty.Length.GetDisplayName(), PropertyAsString(rocket.Length), ERocketProperty.Length.GetSymbol()),
            new(ERocketProperty.Diameter.GetDisplayName(), PropertyAsString(rocket.Diameter), ERocketProperty.Diameter.GetSymbol()),
            new(ERocketProperty.MaxStages.GetDisplayName(), PropertyAsString(rocket.MaxStages), ERocketProperty.MaxStages.GetSymbol()),
            new(ERocketProperty.LaunchCost.GetDisplayName(), PropertyAsString(rocket.LaunchCost), ERocketProperty.LaunchCost.GetSymbol()),
            new(ERocketProperty.LiftoffMass.GetDisplayName(), PropertyAsString(rocket.LiftoffMass), ERocketProperty.LiftoffMass.GetSymbol()),
            new(ERocketProperty.LiftoffThrust.GetDisplayName(), PropertyAsString(rocket.ThrustAtLiftoff), ERocketProperty.LiftoffThrust.GetSymbol()),
            new(ERocketProperty.LeoCapacity.GetDisplayName(), PropertyAsString(rocket.LeoCapacity), ERocketProperty.LeoCapacity.GetSymbol()),
            new(ERocketProperty.GeoCapacity.GetDisplayName(), PropertyAsString(rocket.GeoCapacity), ERocketProperty.GeoCapacity.GetSymbol()),
            new(ERocketProperty.CostPerKgToLeo.GetDisplayName(), PropertyAsString(rocket.CostPerKgToLeo), ERocketProperty.CostPerKgToLeo.GetSymbol()),
            new(ERocketProperty.CostPerKgToGeo.GetDisplayName(), PropertyAsString(rocket.CostPerKgToGeo), ERocketProperty.CostPerKgToGeo.GetSymbol()),
            new(ERocketProperty.SuccessfullLaunches.GetDisplayName(), PropertyAsString(rocket.SuccessfulLaunches), ERocketProperty.SuccessfullLaunches.GetSymbol()),
            new(ERocketProperty.TotalLaunches.GetDisplayName(), PropertyAsString(rocket.TotalLaunches), ERocketProperty.TotalLaunches.GetSymbol()),
            new(ERocketProperty.FirstFlight.GetDisplayName(), PropertyAsString(rocket.FirstFlight), ERocketProperty.FirstFlight.GetSymbol()),
            new(ERocketProperty.LaunchSuccessPercent.GetDisplayName(), PropertyAsString(rocket.LaunchSuccess), ERocketProperty.LaunchSuccessPercent.GetSymbol()),
        };
        
        return new LaunchDetailsVM
        {
            Agency = new AgencyVM
            {
                Name = agency.Name,
                Description = agency.Description,
                ImageUrl = agency.LogoUrl
            },
            Rocket = new RocketVM
            {
                Name = rocket.Name,
                Description = rocket.Description,
                ImageUrl = rocket.ImageUrl,
                Properties = properties
            }
        };
    }
}


