using SpaceHub.Application.Errors;
using SpaceHub.Application.Interfaces;

namespace SpaceHub.Application.Features.Launches;

public record GetLaunchDetailsQuery(Guid Id) : IRequest<Result<LaunchDetailsVM>>;

public class GetLaunchDetailsQueryValidator : AbstractValidator<GetLaunchDetailsQuery>
{
    public GetLaunchDetailsQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

internal class GetLaunchDetailsHandler : IRequestHandler<GetLaunchDetailsQuery, Result<LaunchDetailsVM>>
{
    private readonly IDbContext _db;

    public GetLaunchDetailsHandler(IDbContext db)
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
            .FirstOrDefaultAsync();
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
                ImageUrl = agency.LogoUrl,
                Type = agency.Type,
                CountryCode = agency.CountryCode,
                Administrator = agency.Administrator,
                FoundingYear = agency.FoundingYear
            },
            Rocket = rocket.ToViewModel()
        });
    }
}


