﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpaceHub.Application.Enums;
using SpaceHub.Application.Features.Launches;
using SpaceHub.Application.Features.News;

namespace SpaceHub.Web.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpaceController : ControllerBase
{
    private readonly IMediator _mediator;

    public SpaceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("articles")]
    public async Task<GetNewsResult> GetArticles(string? searchValue, int pageNumber, int itemsPerPage)
    {
        return await _mediator.Send(new GetNewsQuery(searchValue, pageNumber, itemsPerPage));
    }

    [HttpGet("launches")]
    public async Task<GetLaunchesResult> GetLaunches(ETimeFrame timeFrame, string? searchValue, int pageNumber, int itemsPerPage)
    {
        return await _mediator.Send(new GetLaunchesQuery(timeFrame, searchValue, pageNumber, itemsPerPage));
    }
}
