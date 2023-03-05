using SpaceHub.Domain.Models;
using System.Linq.Expressions;

namespace SpaceHub.UnitTests.Domain.Models;

public class RocketTests
{
    private readonly Fixture _fixture = new();
    private readonly OrbitType[] _orbitTypes = Enum.GetValues<OrbitType>();

    private enum OrbitType
    {
        LEO,
        GEO,
    }

    public static IEnumerable<object[]> CostPerKgToOrbit_ShouldBeNullData = new List<object[]>
    {
        new object[] { null, 1 },
        new object[] { 1L, null },
        new object[] { 1L, 0 },
    };

    [Theory]
    [MemberData(nameof(CostPerKgToOrbit_ShouldBeNullData))]
    public void CostPerKgToOrbit_ShouldBeNull_WhenAnyOfThePropertiesAreNotValid(long? launchCost, int? orbitCapacity)
    {
        foreach(var orbitType in _orbitTypes)
        {
            var rocket = _fixture.Build<Rocket>().With(x => x.LaunchCost, launchCost).With(GetOrbitCapacityProperty(orbitType), orbitCapacity).Create();

            GetCostPerKgToOrbit(rocket, orbitType).Should().BeNull();
        }
    }

    public static IEnumerable<object[]> CostPerKgToOrbit_ShouldBeExpectedValueData = new List<object[]>
    {
        new object[] { 1000, 5, 200 },
        new object[] { 10, 6, 1 },
    };

    [Theory]
    [MemberData(nameof(CostPerKgToOrbit_ShouldBeExpectedValueData))]
    public void CostPerKgToOrbit_ShouldBeExpectedValue_WhenPropertiesAreValid(long launchCost, int orbitCapacity, int expected)
    {
        foreach(var orbitType in _orbitTypes)
        {
            var rocket = _fixture.Build<Rocket>().With(x => x.LaunchCost, launchCost).With(GetOrbitCapacityProperty(orbitType), orbitCapacity).Create();

            GetCostPerKgToOrbit(rocket, orbitType).Should().Be(expected);
        }
    }

    [Fact]
    public void LaunchSuccess_ShouldBeNull_WhenTotalLaunchesAreNotLargerThanZero()
    {
        var rocket = _fixture.Build<Rocket>().With(x => x.TotalLaunches, 0).Create();

        rocket.LaunchSuccess.Should().BeNull();
    }

    [Theory]
    [InlineData(3, 5, 60)]
    [InlineData(7, 11, 64)]
    [InlineData(10, 17, 59)]
    public void LaunchSuccess_ShouldBeExpectedValue_WhenTotalLaunchesIsLargerThanZero(int successfulLaunches, int totalLaunches, int expected)
    {
        var rocket = _fixture.Build<Rocket>().With(x => x.SuccessfulLaunches, successfulLaunches).With(x => x.TotalLaunches, totalLaunches).Create();

        rocket.LaunchSuccess.Should().Be(expected);
    }

    private Expression<Func<Rocket, int?>> GetOrbitCapacityProperty(OrbitType orbitType) => orbitType switch
    {
        OrbitType.LEO => x => x.LeoCapacity,
        OrbitType.GEO => (x => x.GeoCapacity)
    };

    private int? GetCostPerKgToOrbit(Rocket rocket, OrbitType orbitType) => orbitType switch
    {
        OrbitType.LEO => rocket.CostPerKgToLeo,
        OrbitType.GEO => rocket.CostPerKgToGeo
    };
}
