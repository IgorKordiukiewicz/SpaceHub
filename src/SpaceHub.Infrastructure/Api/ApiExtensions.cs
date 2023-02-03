namespace SpaceHub.Infrastructure.Api;

public static class ApiExtensions
{
    public static string ToQueryParameter(this DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-ddTHH:mm:ss");
    }
}
