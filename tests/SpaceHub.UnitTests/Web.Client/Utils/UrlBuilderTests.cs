using SpaceHub.Web.Client.Utils;

namespace SpaceHub.UnitTests.Web.Client.Utils;

public class UrlBuilderTests
{
    [Fact]
    public void Url_ShouldReturnRouteWithNoParameters_WhenNoParametersAreAdded()
    {
        var url = new UrlBuilder("api/test").Url;

        url.Should().Be("api/test");
    }

    [Fact]
    public void Url_ShouldReturnRouteWithOneParameter_WhenParameterIsAddedWithAddParameter()
    {
        var url = new UrlBuilder("api/test").AddParameter("foo", 10).Url;

        url.Should().Be("api/test?foo=10");
    }

    [Fact]
    public void Url_ShouldReturnRouteWithManyParameters_WhenParametersAreAddedWithAddParameter()
    {
        var doubleAsString = 22.5.ToString();
        var url = new UrlBuilder("api/test")
            .AddParameter("foo1", 10)
            .AddParameter("foo2", doubleAsString)
            .AddParameter("foo3", "bar").Url;

        url.Should().Be($"api/test?foo1=10&foo2={doubleAsString}&foo3=bar");
    }

    [Fact]
    public void Url_ShouldReturnRouteWithOneParameter_WhenParameterIsAddedWithAddParameters()
    {
        var url = new UrlBuilder("api/test").AddParameters(new (string, string)[] { ("foo", "10") }).Url;

        url.Should().Be("api/test?foo=10");
    }

    [Fact]
    public void Url_ShouldReturnRouteWithManyParameters_WhenParametersAreAddedWithAddParameters()
    {
        var doubleAsString = 22.5.ToString();
        var url = new UrlBuilder("api/test")
            .AddParameters(new (string, string)[]
            {
                ("foo1", "10"),
                ("foo2", doubleAsString),
                ("foo3", "bar")
            }).Url;

        url.Should().Be($"api/test?foo1=10&foo2={doubleAsString}&foo3=bar");
    }
}
