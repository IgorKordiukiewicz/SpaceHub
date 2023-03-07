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
            _fixture.Build<Rocket>().With(x => x.Length, 70.0).With(x => x.LiftoffMass, 200).With(x => x.LaunchCost, 2000).With(r => r.LeoCapacity, 1).Create(),
            _fixture.Build<Rocket>().With(x => x.Length, 40.0).With(x => x.LiftoffMass, 100).With(x => x.LaunchCost, 4000).With(r => r.LeoCapacity, 1).Create(),
            _fixture.Build<Rocket>().With(x => x.Length, 90.0).With(x => x.LiftoffMass, 300).With(x => x.LaunchCost, 5000).With(r => r.LeoCapacity, 1).Create(),
            _fixture.Build<Rocket>().With(x => x.Length, 120.0).With(x => x.LiftoffMass, 200).With(x => x.LaunchCost, 3000).With(r => r.LeoCapacity, 1).Create(),
            _fixture.Build<Rocket>().With(x => x.Length, 50.0).With(x => x.LiftoffMass, 700).With(x => x.LaunchCost, 1000).With(r => r.LeoCapacity, 1).Create(),
        };

        _calculator = new(_rockets);
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
}
