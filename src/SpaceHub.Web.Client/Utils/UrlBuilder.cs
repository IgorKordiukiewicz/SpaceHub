namespace SpaceHub.Web.Client.Utils;

public class UrlBuilder
{
    private string _route = string.Empty;
    private string _queryParameters = string.Empty;

    public string Url => _route + _queryParameters;

    public UrlBuilder(string route)
    {
        _route = route;
    }

    public UrlBuilder AddParameter<T>(string name, T value)
    {
        var prefix = string.IsNullOrEmpty(_queryParameters) ? "?" : "&";
        _queryParameters += $"{prefix}{name}={value}";
        return this;
    }

    public UrlBuilder AddParameters(IEnumerable<(string, string)> parameters)
    {
        foreach(var (name, value) in parameters)
        {
            AddParameter(name, value);
        }
        return this;
    }
}
