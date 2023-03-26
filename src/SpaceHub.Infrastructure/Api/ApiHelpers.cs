using OneOf;
using Refit;
using SpaceHub.Infrastructure.Errors;

namespace SpaceHub.Infrastructure.Api;

public static class ApiHelpers
{
    public static int GetRequiredRequestsCount(int itemsCount, int maxItemsPerRequest)
        => (int)Math.Ceiling((float)itemsCount / maxItemsPerRequest);

    public static OneOf<T, ApiError> GetContentOrError<T>(this IApiResponse<T> response)
    {
        if (!response.IsSuccessStatusCode || response.Content is null)
        {
            return new ApiError(response);
        }

        return response.Content;
    }

    public static string ToQueryParameter(this DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-ddTHH:mm:ss");
    }
}
