using SpaceHub.Application.Errors;
using SpaceHub.Domain.Models;
using SpaceHub.Infrastructure.Data;

namespace SpaceHub.Application.Features.Launches;

public record GetLaunchDetailsQuery(string Id) : IRequest<Result<LaunchDetailsVM>>;

public class GetLaunchDetailsQueryValidator : AbstractValidator<GetLaunchDetailsQuery>
{
    public GetLaunchDetailsQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

internal class GetLaunchDetailsHandler : IRequestHandler<GetLaunchDetailsQuery, Result<LaunchDetailsVM>>
{
    private readonly DbContext _db;

    public GetLaunchDetailsHandler(DbContext db)
    {
        _db = db;
    }

    public async Task<Result<LaunchDetailsVM>> Handle(GetLaunchDetailsQuery request, CancellationToken cancellationToken)
    {
        var launch = await _db.Launches.AsQueryable().FirstOrDefaultAsync(x => x.ApiId == request.Id);
        if (launch is null)
        {
            return Result.Fail<LaunchDetailsVM>(new RecordNotFoundError($"Launch with id {request.Id} not found."));
        }
        
        var agency = await _db.Agencies.AsQueryable().FirstOrDefaultAsync(x => x.ApiId == launch.AgencyApiId);
        if (agency is null)
        {
            return Result.Fail<LaunchDetailsVM>(new RecordNotFoundError($"Agency with id {launch.AgencyApiId} not found."));
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
                LiftoffThrust = x.ThrustAtLiftoff,
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
            return Result.Fail<LaunchDetailsVM>(new RecordNotFoundError($"Rocket with id {launch.RocketApiId} not found."));
        }

        return Result.Ok(new LaunchDetailsVM
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
                Length = rocket.Length,
                Diameter = rocket.Diameter,
                MaxStages = rocket.MaxStages,
                LaunchCost = rocket.LaunchCost,
                LiftoffMass = rocket.LiftoffMass,
                LiftoffThrust = rocket.LiftoffThrust,
                LeoCapacity = rocket.LeoCapacity,
                GeoCapacity = rocket.GeoCapacity,
                CostPerKgToLeo = rocket.CostPerKgToLeo,
                CostPerKgToGeo = rocket.CostPerKgToGeo,
                SuccessfulLaunches = rocket.SuccessfulLaunches,
                TotalLaunches = rocket.TotalLaunches,
                LaunchSuccess = rocket.LaunchSuccess,
                FirstFlight = rocket.FirstFlight is not null ? DateOnly.FromDateTime(rocket.FirstFlight.Value) : null,
            }
        });
    }
}


