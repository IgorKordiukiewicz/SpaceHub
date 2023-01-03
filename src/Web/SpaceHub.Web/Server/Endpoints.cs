using MediatR;
using SpaceHub.Application.Features.Launches;
using SpaceHub.Application.Features.News;
using SpaceHub.Contracts.Enums;

namespace SpaceHub.Web.Server;

public static class EndpointsExtension
{
    public static void AddEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/articles", async (IMediator mediator, string searchValue, int pageNumber, int itemsPerPage) =>
        {
            return await mediator.Send(new GetNewsQuery(searchValue, pageNumber, itemsPerPage));
        });

        app.MapGet("/api/launches", async (IMediator mediator, ETimeFrame timeFrame, string searchValue, int pageNumber, int itemsPerPage) =>
        {
            return await mediator.Send(new GetLaunchesQuery(timeFrame, searchValue, pageNumber, itemsPerPage));
        });

        app.MapGet("/api/launches/{id}", async (IMediator mediator, string id) =>
        {
            return await mediator.Send(new GetLaunchDetailsQuery(id));
        });
    }
}
