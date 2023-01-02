using MediatR;
using Microsoft.Extensions.Caching.Memory;
using SpaceHub.Application.Common;
using SpaceHub.Application.Enums;
using SpaceHub.Contracts.Enums;
using SpaceHub.Contracts.ViewModels;
using SpaceHub.Infrastructure.Api;

namespace SpaceHub.Application.Features.Launches;

public record GetLaunchesQuery(ETimeFrame TimeFrame, string? SearchValue, int PageNumber, int ItemsPerPage) : IRequest<LaunchesVM>;

internal class GetLaunchesHandler : IRequestHandler<GetLaunchesQuery, LaunchesVM>
{
    private readonly IMemoryCache _cache;
    private readonly ILaunchApi _launchApi;

    public GetLaunchesHandler(IMemoryCache cache, ILaunchApi launchApi)
    {
        _cache = cache;
        _launchApi = launchApi;
    }

    public async Task<LaunchesVM> Handle(GetLaunchesQuery request, CancellationToken cancellationToken)
    {
        var offset = Pagination.GetOffset(request.PageNumber, request.ItemsPerPage);
        var timeFrameStr = request.TimeFrame.ToString().ToLower();

        var response = await _cache.GetOrCreateAsync(
            CacheHelpers.GetCacheKeyForRequestWithPages($"{timeFrameStr}_launches", request.SearchValue, offset, request.ItemsPerPage), 
            async entry =>
            {
                return await _launchApi.GetLaunchesAsync(timeFrameStr, request.SearchValue, request.ItemsPerPage, offset);
            });

        // TODO: Add null checks, etc
        var launches = response.Launches.Select(x => new LaunchVM
        {
            Id = x.Id,
            Name = x.Name,
            Status = x.Status.Name,
            Date = x.Date,
            ImageUrl = x.ImageUrl,
            MissionDescription = x.Mission?.Description,
            AgencyName = x.Agency.Name,
            PadLocationName = x.Pad.Location.Name,
            Upcoming = x.Date > DateTime.Now, // TODO: Move to domain?
            TimeToLaunch = x.Date - DateTime.Now
        }).ToList();

        return new LaunchesVM(launches, response.Count);
    }
}
