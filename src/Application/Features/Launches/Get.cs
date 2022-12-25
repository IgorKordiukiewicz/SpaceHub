﻿using MediatR;
using Microsoft.Extensions.Caching.Memory;
using SpaceHub.Application.Common;
using SpaceHub.Application.Enums;
using SpaceHub.Infrastructure.Api;

namespace SpaceHub.Application.Features.Launches;

public record GetLaunchesQuery(ETimeFrame TimeFrame, string? SearchValue, int PageNumber, int ItemsPerPage) : IRequest<GetLaunchesResult>;
public record GetLaunchesResult(List<LaunchViewModel> Launches, int TotalPagesCount);

public class GetLaunchesHandler : IRequestHandler<GetLaunchesQuery, GetLaunchesResult>
{
    private readonly IMemoryCache _cache;
    private readonly ILaunchApi _launchApi;

    public GetLaunchesHandler(IMemoryCache cache, ILaunchApi launchApi)
    {
        _cache = cache;
        _launchApi = launchApi;
    }

    public async Task<GetLaunchesResult> Handle(GetLaunchesQuery request, CancellationToken cancellationToken)
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

        var launches = response.Launches.Select(x => new LaunchViewModel
        {
            Id = x.Id,
            Name = x.Name,
            StatusName = x.Status.Name,
            StatusAbbrevation = x.Status.Abbrevation,
            StatusDescription = x.Status.Description,
            Date= x.Date,
            WindowStart= x.WindowStart,
            WindowEnd= x.WindowEnd,
            ImageUrl= x.ImageUrl,
            MissionName = x.Mission?.Name,
            MissionDescription = x.Mission?.Description,
            AgencyName = x.Agency.Name,
            PadName = x.Pad.Name,
            PadLocationName = x.Pad.Location.Name,
        }).ToList();

        return new GetLaunchesResult(launches, response.Count);
    }
}

public record LaunchViewModel
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string StatusName { get; init; }
    public string StatusDescription { get; init; }
    public string StatusAbbrevation { get; init; }
    public DateTime? Date { get; init; }
    public DateTime? WindowStart { get; init; }
    public DateTime? WindowEnd { get; init; }
    public string ImageUrl { get; init; }
    public string? MissionName { get; init; }
    public string? MissionDescription { get; init; }
    public string AgencyName { get; init; }
    public string PadName { get; init; }
    public string PadLocationName { get; init; }
}

public record LaunchExpandedViewModel
{

}
