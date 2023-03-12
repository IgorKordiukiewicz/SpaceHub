using OneOf;
using Refit;
using SpaceHub.Application.Errors;

namespace SpaceHub.Application.Common;

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
}
