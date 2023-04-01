using SpaceHub.Application.Interfaces;
using SpaceHub.Contracts.Models;

namespace SpaceHub.Application.Features.Rockets;

public record GetRocketsQuery(string SearchValue, Pagination Pagination) : IRequest<Result<RocketsVM>>;

public class GetRocketsQueryValidator : AbstractValidator<GetRocketsQuery>
{
    public GetRocketsQueryValidator()
    {
        RuleFor(x => x.SearchValue).NotNull();
        RuleFor(x => x.Pagination).SetValidator(new PaginationValidator());
    }
}

internal sealed class GetRocketsHandler : IRequestHandler<GetRocketsQuery, Result<RocketsVM>>
{
    private readonly IDbContext _db;

    public GetRocketsHandler(IDbContext db)
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

        var rockets = await query.Skip(request.Pagination.Offset)
            .Take(request.Pagination.ItemsPerPage)
            .ToListAsync();

        if (!rockets.Any())
        {
            return new RocketsVM(new List<RocketVM>(), totalPagesCount);
        }

        var rocketsVMs = new List<RocketVM>();
        foreach(var rocket in rockets)
        {
            rocketsVMs.Add(rocket.ToViewModel());
        }

        return new RocketsVM(rocketsVMs, totalPagesCount);
    }
}
