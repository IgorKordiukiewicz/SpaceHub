using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpaceHub.Application.Features.Launches;
using SpaceHub.Application.Features.News;
using SpaceHub.Contracts.Enums;
using SpaceHub.Contracts.ViewModels;

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
    public async Task<ArticlesVM> GetArticles(string? searchValue, int pageNumber, int itemsPerPage)
    {
        return await _mediator.Send(new GetNewsQuery(searchValue, pageNumber, itemsPerPage));
    }

    [HttpGet("launches")]
    public async Task<LaunchesVM> GetLaunches(ETimeFrame timeFrame, string? searchValue, int pageNumber, int itemsPerPage)
    {
        return await _mediator.Send(new GetLaunchesQuery(timeFrame, searchValue, pageNumber, itemsPerPage));
    }

    [HttpGet("launches/{id}")]
    public async Task<LaunchDetailsVM> GetLaunchDetails(string id)
    {
        var res = await _mediator.Send(new GetLaunchDetailsQuery(id));
        return res;
    }
}
