using SpaceHub.Contracts.Enums;
using SpaceHub.Contracts.Models;
using SpaceHub.Infrastructure.Data;

namespace SpaceHub.Application.Features.Launches;

public record GetLaunchesQuery(ETimeFrame TimeFrame, string SearchValue, Pagination Pagination) : IRequest<Result<LaunchesVM>>;

public class GetLaunchesQueryValidator : AbstractValidator<GetLaunchesQuery>
{
    public GetLaunchesQueryValidator()
    {
        RuleFor(x => x.TimeFrame).NotNull().IsInEnum();
        RuleFor(x => x.SearchValue).NotNull();
        RuleFor(x => x.Pagination).SetValidator(new PaginationValidator());
    }
}

internal class GetLaunchesHandler : IRequestHandler<GetLaunchesQuery, Result<LaunchesVM>>
{
    private readonly DbContext _db;
    private readonly IDateTimeProvider _dateTimeProvider;

    public GetLaunchesHandler(DbContext db, IDateTimeProvider dateTimeProvider)
    {
        _db = db;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<LaunchesVM>> Handle(GetLaunchesQuery request, CancellationToken cancellationToken)
    {
        var now = _dateTimeProvider.Now();

        var query = _db.Launches.AsQueryable()
            .Where(x => x.Name.ToLower().Contains(request.SearchValue.ToLower()));
        if(request.TimeFrame == ETimeFrame.Upcoming)
        {
            query = query.Where(x => x.Date > now)
                .OrderBy(x => x.Date);
        }
        else
        {
            query = query.Where(x => x.Date <= now)
                .OrderByDescending(x => x.Date);
        }
        
        var count = await query.CountAsync();
        var totalPagesCount = request.Pagination.GetPagesCount(count);
        
        var launches = (await query.Skip(request.Pagination.Offset)
            .Take(request.Pagination.ItemsPerPage)
            .ToListAsync())
            .Select(x => new LaunchVM
            {
                Id = x.ApiId,
                Name = x.Name,
                Status = x.Status,
                Date = x.Date,
                ImageUrl = x.ImageUrl,
                MissionDescription = x.Mission != null ? x.Mission.Description : string.Empty,
                AgencyName = x.AgencyName,
                PadLocationName = x.Pad.LocationName,
                Upcoming = x.Date > now,
                TimeToLaunch = x.Date - now,
                VideosUrls = x.Videos.Select(xx => xx.Url).ToList()
            }).ToList();

        return Result.Ok(new LaunchesVM(launches, totalPagesCount));
    }
}
