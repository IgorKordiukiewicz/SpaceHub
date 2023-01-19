namespace SpaceHub.Application.Common;

public static class ApiHelpers
{
    public static int GetRequiredRequestsCount(int itemsCount, int maxItemsPerRequest)
        => (int)Math.Ceiling((float)itemsCount / maxItemsPerRequest);
}
