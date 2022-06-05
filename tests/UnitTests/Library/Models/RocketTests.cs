using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Library.Models;

namespace UnitTests.Library.Models
{
    public class RocketTests
    {
        [Theory]
        [InlineData(5, 5, 100)]
        [InlineData(5, 10, 50)]
        [InlineData(5, 11, 45)]
        [InlineData(5, 9, 56)]
        public void LaunchSuccessPercent_ShouldBeCalculatedProperly_WhenTotalLaunchCountIsBiggerThanZero(int successfulLaunches, int totalLaunchCount, int expected)
        {
            Rocket rocket = new()
            {
                Details = new()
                {
                    SuccessfulLaunches = successfulLaunches,
                    TotalLaunchCount = totalLaunchCount,
                }
            };

            var result = rocket.Details.LaunchSuccessPercent;

            result.Should().Be(expected);
        }

        [Fact]
        public void LaunchSuccesPercent_ShouldBeZero_WhenTotalLaunchCountIsZero()
        {
            Rocket rocket = new()
            {
                Details = new()
                {
                    TotalLaunchCount = 0,
                }
            };

            var result = rocket.Details.LaunchSuccessPercent;

            result.Should().Be(0);
        }

        [Theory]
        [InlineData(true, 1000, 10, 100)]
        [InlineData(true, 5, 2, 2)]
        [InlineData(false, 1000, 10, 100)]
        [InlineData(false, 5, 2, 2)]
        public void CostPerKgToLeo_ShouldBeCalculatedProperly_WhenLaunchCostAndLeoCapacityAreNotNullAndLeoCapacityIsBiggerThanZero(
            bool leo, int launchCost, int orbitCapacity, int expected)
        {
            Rocket rocket = new()
            {
                Details = new()
                {
                    LaunchCost = launchCost,
                    LeoCapacity = orbitCapacity,
                    GeoCapacity = orbitCapacity
                }
            };

            var result = leo ? rocket.Details.CostPerKgToLeo : rocket.Details.CostPerKgToGeo;

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CostPerKgToOrbit_ShouldBeNull_WhenLaunchCostIsNull(bool leo)
        {
            Rocket rocket = new()
            {
                Details = new()
                {
                    LaunchCost = null,
                    LeoCapacity = 10,
                    GeoCapacity = 10,
                }
            };

            var result = leo ? rocket.Details.CostPerKgToLeo : rocket.Details.CostPerKgToGeo;

            result.Should().BeNull();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CostPerKgToOrbit_ShouldBeNull_WhenOrbitCapacityIsNull(bool leo)
        {
            Rocket rocket = new()
            {
                Details = new()
                {
                    LaunchCost = 10,
                    LeoCapacity = null,
                    GeoCapacity = null,
                }
            };

            var result = leo ? rocket.Details.CostPerKgToLeo : rocket.Details.CostPerKgToGeo;

            result.Should().BeNull();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CostPerKgToOrbit_ShouldBeNull_WhenOrbitCapacityIsSmallerOrEqualToZero(bool leo)
        {
            Rocket rocket = new()
            {
                Details = new()
                {
                    LaunchCost = 10,
                    LeoCapacity = 0,
                    GeoCapacity = 0,
                }
            };

            var result = leo ? rocket.Details.CostPerKgToLeo : rocket.Details.CostPerKgToGeo;

            result.Should().BeNull();
        }
    }
}
