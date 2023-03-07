using SpaceHub.Contracts.Enums;
using SpaceHub.Domain;
using SpaceHub.Domain.Models;

namespace SpaceHub.UnitTests.Domain;

public class RocketComparisonCalculatorTests
{
    private readonly Fixture _fixture = new();
    private readonly double _precision = 0.01;
    private readonly List<Rocket> _rockets;
    private readonly RocketComparisonCalculator _calculator;

    public RocketComparisonCalculatorTests()
    {
        _rockets = new()
        {
            CreateRocket(70.0, 200, 2000, "Rocket 1"),
            CreateRocket(40.0, 100, 4000, "Rocket 2"),
            CreateRocket(90.0, 300, 5000, "Rocket 3"),
            CreateRocket(50.0, 700, 1000, "Rocket 4"),
            CreateRocket(120.0, 200, 3000, "Rocket 5"),
            CreateRocket(120.0, 200, 3000, "Rocket 5"),
            CreateRocket(120.0, 200, 3000, "Rocket 6"),
        };

        _calculator = new(_rockets);
    }

    private Rocket CreateRocket(double length, int liftoffMass, long launchCost, string name)
    {
        return _fixture.Build<Rocket>()
            .With(x => x.Length, length)
            .With(x => x.LiftoffMass, liftoffMass)
            .With(x => x.LaunchCost, launchCost)
            .With(x => x.LeoCapacity, 1)
            .With(x => x.Name, name)
            .Create();
    }

    [Fact]
    public void CalculatePropertyRanking_ShouldReturnDefaultRanking_WhenPropertyIsInvalid()
    {
        var property = (ERocketComparisonProperty)(-1);
        var result = _calculator.CalculatePropertyRanking(property, _rockets);

        using(new AssertionScope())
        {
            result.Value.Should().BeNull();
            result.Rank.Should().BeNull();
            result.Fraction.Should().BeApproximately(0.0, _precision);
        }
    }

    [Fact]
    public void CalculatePropertyRanking_ShouldReturnDefaultRanking_WhenRocketsHaveNullPropertyValues()
    {
        var result = _calculator.CalculatePropertyRanking(ERocketComparisonProperty.Length, new Rocket[]
        {
            _fixture.Build<Rocket>().With(x => x.Length, (double?)null).Create(),
            _fixture.Build<Rocket>().With(x => x.Length, (double?)null).Create()
        });

        using (new AssertionScope())
        {
            result.Value.Should().BeNull();
            result.Rank.Should().BeNull();
            result.Fraction.Should().BeApproximately(0.0, _precision);
        }
    }

    [Theory]
    // Value [70] : 0.0 - 40, 0.25 - 50, 0.5 - [70], 0.75 - 90, 1.0 - 120
    [InlineData(ERocketComparisonProperty.Length, 0.5, 3.0)] 
    // Value [200] : 0.0 - 100, 0.33 - [200], 0.67 - 300, 1.0 - 700
    [InlineData(ERocketComparisonProperty.LiftoffMass, 0.33, 3.0)]
    // Value [2000] : 0.0 - 5000, 0.25 - 4000, 0.5 - 3000, 0.75 - [2000], 1.0 - 1000 (descending)
    [InlineData(ERocketComparisonProperty.CostPerKgToLeo, 0.75, 2.0)] 
    public void CalculatePropertyRanking_ShouldReturnCorrectFractionAndRankForOneRocket_WhenPropertyIsValid(ERocketComparisonProperty property, double expectedFraction, double expectedRank)
    {
        var result = _calculator.CalculatePropertyRanking(property, new Rocket[] { _rockets[0] });

        using(new AssertionScope())
        {
            result.Fraction.Should().BeApproximately(expectedFraction, _precision);
            result.Rank.Should().BeApproximately(expectedRank, _precision);
        }
    }

    [Theory]
    // Value [(90 + 40) / 2 = 65] : 0.0 - 40, 0.25 - 50, 0.5 - 70, 0.75 - 90, 1.0 - 120 -> between 0.25 & 0.5 -> 0.375
    [InlineData(ERocketComparisonProperty.Length, 0.375, 3.5)]
    // Value [(100 + 300) / 2 = 200] : 0.0 - 100, 0.33 - [200], 0.67 - 300, 1.0 - 700
    [InlineData(ERocketComparisonProperty.LiftoffMass, 0.33, 3.0)]
    // Value [(4000 + 5000) / 2 = 4500] : 0.0 - 5000, 0.25 - 4000, 0.5 - 3000, 0.75 - 2000, 1.0 - 1000 (descending) -> between 0.0 & 0.25 -> 0.125
    [InlineData(ERocketComparisonProperty.CostPerKgToLeo, 0.125, 4.5)]
    public void CalculatePropertyRanking_ShouldReturnCorrectFractionAndRankForGroupOfRockets_WhenPropertyIsValid(ERocketComparisonProperty property, double expectedFraction, double expectedRank)
    {
        var result = _calculator.CalculatePropertyRanking(property, new Rocket[] { _rockets[1], _rockets[2] });

        using (new AssertionScope())
        {
            result.Fraction.Should().BeApproximately(expectedFraction, _precision);
            result.Rank.Should().BeApproximately(expectedRank, _precision);
        }
    }

    [Fact]
    public void GetTopValues_ShouldReturnCorrectlyOrderedItems_WhenPropertyOrderingIsAscending()
    {
        var result = _calculator.GetTopValues(2);
    
        var rocketsOrdered = _rockets.OrderByDescending(x => x.Length).ToList();
    
        using(new AssertionScope())
        {
            var topValues = result[ERocketComparisonProperty.Length];
            topValues.Count.Should().Be(2);

            void AssertTopValue(int index, double expectedValue, string[] expectedNames)
            {
                topValues![index].Value.Should().BeApproximately(expectedValue, _precision);
                topValues![index].Names.Should().BeEquivalentTo(expectedNames);
            }
            AssertTopValue(0, 120.0, new[] { "Rocket 5", "Rocket 6" });
            AssertTopValue(1, 90.0, new[] { "Rocket 3" });
        }
    }
    
    [Fact]
    public void GetTopValues_ShouldReturnCorrectlyOrderedItems_WhenPropertyOrderingIsDescending()
    {
        var result = _calculator.GetTopValues(3);
    
        var rocketsOrdered = _rockets.OrderByDescending(x => x.Length).ToList();
    
        using (new AssertionScope())
        {
            var topValues = result[ERocketComparisonProperty.CostPerKgToLeo];
            topValues.Count.Should().Be(3);

            void AssertTopValue(int index, int expectedValue, string[] expectedNames)
            {
                topValues![index].Value.Should().Be(expectedValue);
                topValues![index].Names.Should().BeEquivalentTo(expectedNames);
            }
            AssertTopValue(0, 1000, new[] { "Rocket 4" });
            AssertTopValue(1, 2000, new[] { "Rocket 1" });
            AssertTopValue(2, 3000, new[] { "Rocket 5", "Rocket 6" });
        }
    }

    [Fact]
    public void GetTopValues_ShouldReturnAtMostTheNumberOfDifferentValues_WhenCountParameterIsLargerThanCountOfDifferentValues()
    {
        var count = _rockets.DistinctBy(x => x.Length).Count();
        var result = _calculator.GetTopValues(count + 10);

        result[ERocketComparisonProperty.Length].Count.Should().Be(count);
    }

    [Fact]
    public void GetTopValues_ShouldReturnDataForAllPropertyTypes()
    {
        var result = _calculator.GetTopValues(1);

        using(new AssertionScope())
        {
            foreach(var propertyType in Enum.GetValues<ERocketComparisonProperty>())
            {
                result.ContainsKey(propertyType).Should().BeTrue();
                result[propertyType].Should().NotBeEmpty();
            }
        }
    }
}
