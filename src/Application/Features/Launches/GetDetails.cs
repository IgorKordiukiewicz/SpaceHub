using MediatR;
using Microsoft.Extensions.Caching.Memory;
using SpaceHub.Application.Enums;
using SpaceHub.Application.Extensions;
using SpaceHub.Contracts.ViewModels;
using SpaceHub.Domain;
using SpaceHub.Infrastructure.Api;

namespace SpaceHub.Application.Features.Launches;

public record GetLaunchDetailsQuery(string Id) : IRequest<LaunchDetailsVM>;

internal class GetLaunchDetailsHandler : IRequestHandler<GetLaunchDetailsQuery, LaunchDetailsVM>
{
    private readonly IMemoryCache _cache;
    private readonly ILaunchApi _launchApi;

    public GetLaunchDetailsHandler(IMemoryCache cache, ILaunchApi launchApi)
    {
        _cache = cache;
        _launchApi = launchApi;
    }

    public async Task<LaunchDetailsVM> Handle(GetLaunchDetailsQuery request, CancellationToken cancellationToken)
    {
        var result = await _cache.GetOrCreateAsync("launch" + request.Id, async entry =>
        {
            return await _launchApi.GetLaunchDetailsAsync(request.Id);
        });

        var rocketConfig = result.Rocket.Configuration;

        static string PropertyAsString<T>(T? value)
        {
            return value is null ? "-" : value.ToString();
        }

        long? launchCost = long.TryParse(rocketConfig.LaunchCost, out var val) ? val : null;

        var properties = new List<RocketPropertyVM>()
        {
            new(ERocketProperty.Length.GetDisplayName(), PropertyAsString(rocketConfig.Length), ERocketProperty.Length.GetSymbol()),
            new(ERocketProperty.Diameter.GetDisplayName(), PropertyAsString(rocketConfig.Diameter), ERocketProperty.Diameter.GetSymbol()),
            new(ERocketProperty.MaxStages.GetDisplayName(), PropertyAsString(rocketConfig.MaxStage), ERocketProperty.MaxStages.GetSymbol()),
            new(ERocketProperty.LaunchCost.GetDisplayName(), PropertyAsString(launchCost), ERocketProperty.LaunchCost.GetSymbol()),
            new(ERocketProperty.LiftoffMass.GetDisplayName(), PropertyAsString(rocketConfig.LaunchMass), ERocketProperty.LiftoffMass.GetSymbol()),
            new(ERocketProperty.LiftoffThrust.GetDisplayName(), PropertyAsString(rocketConfig.ThrustAtLiftoff), ERocketProperty.LiftoffThrust.GetSymbol()),
            new(ERocketProperty.LeoCapacity.GetDisplayName(), PropertyAsString(rocketConfig.LeoCapacity), ERocketProperty.LeoCapacity.GetSymbol()),
            new(ERocketProperty.GeoCapacity.GetDisplayName(), PropertyAsString(rocketConfig.GeoCapacity), ERocketProperty.GeoCapacity.GetSymbol()),
            new(ERocketProperty.CostPerKgToLeo.GetDisplayName(), PropertyAsString(RocketHelper.GetCostPerKgToOrbit(launchCost, rocketConfig.LeoCapacity)), ERocketProperty.CostPerKgToLeo.GetSymbol()),
            new(ERocketProperty.CostPerKgToGeo.GetDisplayName(), PropertyAsString(RocketHelper.GetCostPerKgToOrbit(launchCost, rocketConfig.GeoCapacity)), ERocketProperty.CostPerKgToGeo.GetSymbol()),
            new(ERocketProperty.SuccessfullLaunches.GetDisplayName(), PropertyAsString(rocketConfig.SuccessfulLaunches), ERocketProperty.SuccessfullLaunches.GetSymbol()),
            new(ERocketProperty.TotalLaunches.GetDisplayName(), PropertyAsString(rocketConfig.TotalLaunchCount), ERocketProperty.TotalLaunches.GetSymbol()),
            new(ERocketProperty.FirstFlight.GetDisplayName(), PropertyAsString(rocketConfig.FirstFlight), ERocketProperty.FirstFlight.GetSymbol()),
            new(ERocketProperty.LaunchSuccessPercent.GetDisplayName(), PropertyAsString(RocketHelper.GetLaunchSuccessPercent(rocketConfig.SuccessfulLaunches, rocketConfig.TotalLaunchCount)), 
                ERocketProperty.LaunchSuccessPercent.GetSymbol()),
        };

        return new LaunchDetailsVM
        {
            Agency = new AgencyVM
            {
                Name = result.Agency.Name,
                Description = result.Agency.Description,
                ImageUrl = result.Agency.LogoUrl
            },
            Rocket = new RocketVM
            {
                Name = result.Rocket.Configuration.Name,
                Description = result.Rocket.Configuration.Description,
                ImageUrl = result.Rocket.Configuration.ImageUrl,
                Properties = properties
            }
        };
    }
}


