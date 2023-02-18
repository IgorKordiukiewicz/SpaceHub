using SpaceHub.Infrastructure.Data;

namespace SpaceHub.Application.Features.Rockets;

public record GetRocketsComparisonQuery : IRequest<Result<RocketsComparisonVM>>;

internal class GetRocketsComparisonHandler : IRequestHandler<GetRocketsComparisonQuery, Result<RocketsComparisonVM>>
{
    private readonly DbContext _db;

    public GetRocketsComparisonHandler(DbContext db)
    {
        _db = db;
    }

    public Task<Result<RocketsComparisonVM>> Handle(GetRocketsComparisonQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
