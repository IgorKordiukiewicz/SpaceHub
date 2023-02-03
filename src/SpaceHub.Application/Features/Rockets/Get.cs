using FluentResults;
using FluentValidation;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SpaceHub.Application.Common;
using SpaceHub.Contracts.ViewModels;
using SpaceHub.Domain.Enums;
using SpaceHub.Domain.Extensions;
using SpaceHub.Domain.Models;
using SpaceHub.Infrastructure.Data;

namespace SpaceHub.Application.Features.Rockets;

public record GetRocketsQuery(string SearchValue, int PageNumber, int ItemsPerPage) : IRequest<Result<RocketsVM>>;

public class GetRocketsQueryValidator : AbstractValidator<GetRocketsQuery>
{
    public GetRocketsQueryValidator()
    {
        RuleFor(x => x.SearchValue).NotNull();
        RuleFor(x => x.PageNumber).NotNull().GreaterThanOrEqualTo(1);
        RuleFor(x => x.ItemsPerPage).NotNull().GreaterThanOrEqualTo(1);
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
        var offset = Pagination.GetOffset(request.PageNumber, request.ItemsPerPage);
        var now = DateTime.UtcNow;

        var query = _db.Rockets.AsQueryable()
            .Where(x => x.Name.ToLower().Contains(request.SearchValue.ToLower()))
            .OrderBy(x => x.Name);

        var count = await query.CountAsync();
        var totalPagesCount = Pagination.GetPagesCount(count, request.ItemsPerPage);

        // TODO: Code temporarily copied from launches/getDetails, refactor it later
        var rockets = await query.Skip(offset)
            .Take(request.ItemsPerPage)
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
                ThrustAtLiftoff = x.ThrustAtLiftoff,
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

        static string PropertyAsString<T>(T? value)
        {
            return value is null ? "-" : value.ToString();
        }

        foreach(var rocket in rockets)
        {
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

            rocketsVMs.Add(new()
            {
                Name = rocket.Name,
                Description = rocket.Description,
                ImageUrl = rocket.ImageUrl,
                Properties = properties
            });
        }

        return new RocketsVM(rocketsVMs, totalPagesCount);
    }
}
