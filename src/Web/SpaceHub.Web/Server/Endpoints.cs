using LanguageExt.Common;
using MediatR;
using SpaceHub.Application.Exceptions;
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
            var result = await mediator.Send(new GetNewsQuery(searchValue, pageNumber, itemsPerPage));
            return result.ToHttpResult();
        });

        app.MapGet("/api/launches", async (IMediator mediator, ETimeFrame timeFrame, string searchValue, int pageNumber, int itemsPerPage) =>
        {
            var result = await mediator.Send(new GetLaunchesQuery(timeFrame, searchValue, pageNumber, itemsPerPage));
            return result.ToHttpResult();
        });

        app.MapGet("/api/launches/{id}", async (IMediator mediator, string id) =>
        {
            var result = await mediator.Send(new GetLaunchDetailsQuery(id));
            return result.ToHttpResult();
        });
    }

    private static IResult ToHttpResult<T>(this Result<T> handlerResult)
    {
        return handlerResult.Match(value =>
        {
            return Results.Ok(value);
        }, exception =>
        {
            if (exception is RecordNotFoundException)
            {
                return Results.NotFound(exception.Message);
            }
            else if(exception is ValidationException)
            {
                return Results.BadRequest(exception.Message);
            }

            return Results.StatusCode(500);
        });
    }
}
