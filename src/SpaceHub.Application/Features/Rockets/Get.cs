using SpaceHub.Contracts.Utils;
using SpaceHub.Domain.Models;
using SpaceHub.Infrastructure.Data;

namespace SpaceHub.Application.Features.Rockets;

public record GetRocketsQuery(string SearchValue, Pagination Pagination) : IRequest<Result<RocketsVM>>;

public class GetRocketsQueryValidator : AbstractValidator<GetRocketsQuery>
{
    public GetRocketsQueryValidator()
    {
        RuleFor(x => x.SearchValue).NotNull();
        RuleFor(x => x.Pagination).SetValidator(new PaginationParametersValidator());
    }
}

internal class GetRocketsHandler : IRequestHandler<GetRocketsQuery, Result<RocketsVM>>
{
    private readonly DbContext _db;

    public GetRocketsHandler(DbContext db)
    {
        _db = db;
    }

    public async Task<Result<RocketsVM>> Handle(GetRocketsQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var query = _db.Rockets.AsQueryable()
            .Where(x => x.Name.ToLower().Contains(request.SearchValue.ToLower()))
            .OrderBy(x => x.Name);

        var count = await query.CountAsync();
        var totalPagesCount = request.Pagination.GetPagesCount(count);

        // TODO: Move rocket mappings to /Common ?
        var rockets = await query.Skip(request.Pagination.Offset)
            .Take(request.Pagination.ItemsPerPage)
            .Select(x => new Rocket
            {
                ApiId = x.ApiId,
                Name = x.Name,
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
            }).ToListAsync();

        if (!rockets.Any())
        {
            return new RocketsVM(new List<RocketVM>(), totalPagesCount);
        }

        var rocketsVMs = new List<RocketVM>();
        foreach(var rocket in rockets)
        {
            rocketsVMs.Add(new()
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
                FirstFlight = rocket.FirstFlight,
            });
        }

        return new RocketsVM(rocketsVMs, totalPagesCount);
    }
}
