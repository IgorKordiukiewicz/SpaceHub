using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpaceHub.Application.Errors;
using SpaceHub.Application.Features.Launches;
using SpaceHub.Application.Features.News;
using SpaceHub.Application.Features.Rockets;
using SpaceHub.Contracts.Enums;
using SpaceHub.Contracts.Models;

namespace SpaceHub.Web.Server;

public static class EndpointsExtension
{
    public static void AddEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/articles", async (IMediator mediator, string searchValue, [AsParameters] Pagination pagination) =>
        {
            var result = await mediator.Send(new GetNewsQuery(searchValue, pagination));
            return result.ToHttpResult();
        });

        app.MapGet("/api/launches", async (IMediator mediator, ETimeFrame timeFrame, string searchValue, [AsParameters] Pagination pagination) =>
        {
            var result = await mediator.Send(new GetLaunchesQuery(timeFrame, searchValue, pagination));
            return result.ToHttpResult();
        });

        app.MapGet("/api/launches/{id}", async (IMediator mediator, string id) =>
        {
            var result = await mediator.Send(new GetLaunchDetailsQuery(id));
            return result.ToHttpResult();
        });

        app.MapGet("/api/rockets", async (IMediator mediator, string searchValue, [AsParameters] Pagination pagination) =>
        {
            var result = await mediator.Send(new GetRocketsQuery(searchValue, pagination));
            return result.ToHttpResult();
        });

        // TODO: Since this endpoint only gets data and does not modify it, having it as a POST request, kinda violates REST
        // maybe consider sending the comparisonGroups as a query string (maybe encoded?)
        app.MapPost("/api/rockets/comparison", async (IMediator mediator, [FromBody] IEnumerable<RocketsComparisonDataset> comparisonGroups) =>
        {
            var result = await mediator.Send(new GetRocketsComparisonQuery(comparisonGroups));
            return result.ToHttpResult();
        });

        app.MapGet("/api/rockets/comparison_meta", async (IMediator mediator, int topValuesCount) =>
        {
            var result = await mediator.Send(new GetRocketsComparisonMetaQuery(topValuesCount));
            return result.ToHttpResult();
        });
    }

    private static IResult ToHttpResult<T>(this Result<T> handlerResult)
    {
        if(!handlerResult.IsFailed)
        {
            return Results.Ok(handlerResult.Value);
        }

        return GetErrorResult(handlerResult.Errors);
    }

    private static IResult ToHttpResult(this Result handlerResult)
    {
        if(!handlerResult.IsFailed)
        {
            return Results.Ok();
        }

        return GetErrorResult(handlerResult.Errors);
    }

    private static IResult GetErrorResult(List<IError> errors)
    {
        var error = errors.FirstOrDefault();
        return error switch
        {
            RecordNotFoundError => Results.NotFound(error.Message),
            ValidationError => Results.BadRequest(error.Message),
            ApiError => Results.StatusCode(500),
            _ => Results.StatusCode(500)
        };
    }
}
