using SpaceHub.Domain;

namespace SpaceHub.UnitTests.Domain;

public class RocketHelperTests
{
    [Theory]
    [InlineData(10000L, 1000, 10)]
    [InlineData(20000L, 1000, 20)]
    [InlineData(10000L, 1001, 9)]
    [InlineData(20000L, 1001, 19)]
    public void GetCostPerKgToOrbit_ShouldReturnCostPerKg_WhenInputsAreValid(long launchCost, int orbitCapacity, int expected)
    {
        var result = RocketHelper.GetCostPerKgToOrbit(launchCost, orbitCapacity);

        result.Should().Be(expected);
    }

    [Fact]
    public void GetCostPerKgToOrbit_ShouldReturnNull_WhenLaunchCostIsNull()
    {
        long? launchCost = null;
        var orbitCapacity = 1000;

        var result = RocketHelper.GetCostPerKgToOrbit(launchCost, orbitCapacity);

        result.Should().BeNull();
    }

    [Fact]
    public void GetCostPerKgToOrbit_ShouldReturnNull_WhenOrbitCapacityIsNull()
    {
        var launchCost = 10000L;
        int? orbitCapacity = null;

        var result = RocketHelper.GetCostPerKgToOrbit(launchCost, orbitCapacity);

        result.Should().BeNull();
    }

    [Fact]
    public void GetCostPerKgToOrbit_ShouldReturnNull_WhenOrbitCapacityIsZero()
    {
        var launchCost = 10000L;
        var orbitCapacity = 0;

        var result = RocketHelper.GetCostPerKgToOrbit(launchCost, orbitCapacity);

        result.Should().BeNull();
    }

    [Theory]
    [InlineData(3, 5, 60)]
    [InlineData(7, 11, 64)]
    [InlineData(10, 17, 59)]
    public void GetLaunchSuccessPercent_ShouldReturnSuccessPercentage_WhenInputsAreValid(int successfulLaunches, int totalLaunches, int expected)
    {
        var result = RocketHelper.GetLaunchSuccessPercent(successfulLaunches, totalLaunches);

        result.Should().Be(expected);
    }

    [Fact]
    public void GetLaunchSuccessPercent_ShouldReturnZero_WhenTotalLaunchesIsZero()
    {
        var successfulLaunches = 3;
        var totalLaunches = 0;

        var result = RocketHelper.GetLaunchSuccessPercent(successfulLaunches, totalLaunches);

        result.Should().Be(0);
    }
}
