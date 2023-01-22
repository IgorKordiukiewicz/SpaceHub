﻿using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SpaceHub.Application.Common;
using SpaceHub.Application.Exceptions;
using SpaceHub.Contracts.Enums;
using SpaceHub.Contracts.ViewModels;
using SpaceHub.Infrastructure.Api;
using SpaceHub.Infrastructure.Data;

namespace SpaceHub.Application.Features.Launches;

public record GetLaunchesQuery(ETimeFrame TimeFrame, string SearchValue, int PageNumber, int ItemsPerPage) : IRequest<Result<LaunchesVM>>;

internal class GetLaunchesHandler : IRequestHandler<GetLaunchesQuery, Result<LaunchesVM>>
{
    private readonly DbContext _db;

    public GetLaunchesHandler(DbContext db)
    {
        _db = db;
    }

    public async Task<Result<LaunchesVM>> Handle(GetLaunchesQuery request, CancellationToken cancellationToken)
    {
        // TODO: Refactor validation -> use FluentValidiation, also the query params can't be null
        if(request.PageNumber <= 0)
        {
            return new Result<LaunchesVM>(new ValidationException("Page number has to be > 0."));
        }
        if(request.ItemsPerPage <= 0)
        {
            return new Result<LaunchesVM>(new ValidationException("Items per page have to be > 0."));
        }

        var offset = Pagination.GetOffset(request.PageNumber, request.ItemsPerPage);
        var now = DateTime.UtcNow;
        
        var query = _db.Launches.AsQueryable();
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
        
        var count = query.Count();
        var totalPagesCount = Pagination.GetPagesCount(count, request.ItemsPerPage);
        
        var launches = await query.Skip(offset)
            .Take(request.ItemsPerPage)
            .Select(x => new LaunchVM
            {
                Id = x.ApiId,
                Name = x.Name,
                Status = x.Status,
                Date = x.Date,
                ImageUrl = x.ImageUrl,
                MissionDescription = x.Mission == null ? "" : x.Mission.Description,
                AgencyName = "",
                PadLocationName = x.Pad.LocationName,
                Upcoming = x.Date > now, // TODO: Move to domain?
                TimeToLaunch = x.Date - now
            })
            .ToListAsync();
        
        return new LaunchesVM(launches, totalPagesCount);
    }
}
