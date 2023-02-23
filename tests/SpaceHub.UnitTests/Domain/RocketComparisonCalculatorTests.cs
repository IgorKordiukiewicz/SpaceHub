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
            _fixture.Build<Rocket>().With(x => x.Length, 70.0).With(x => x.LiftoffMass, 200).With(x => x.LaunchCost, 2000).Create(),
            _fixture.Build<Rocket>().With(x => x.Length, 40.0).With(x => x.LiftoffMass, 100).With(x => x.LaunchCost, 4000).Create(),
            _fixture.Build<Rocket>().With(x => x.Length, 90.0).With(x => x.LiftoffMass, 300).With(x => x.LaunchCost, 5000).Create(),
            _fixture.Build<Rocket>().With(x => x.Length, 120.0).With(x => x.LiftoffMass, 200).With(x => x.LaunchCost, 3000).Create(),
            _fixture.Build<Rocket>().With(x => x.Length, 50.0).With(x => x.LiftoffMass, 700).With(x => x.LaunchCost, 1000).Create(),
        };

        _calculator = new(_rockets);
    }

    [Fact]
    public void CalculateFraction_ShouldReturnZero_WhenPropertyIsInvalid()
    {
        var property = (ERocketComparisonProperty)(-1);
        var result = _calculator.CalculateFraction(property, _rockets);

        result.Should().BeApproximately(0.0, _precision);
    }


    [Fact]
    public void CalculateFraction_ShouldReturnZero_WhenRocketsHaveNullPropertyValues()
    {
        var result = _calculator.CalculateFraction(ERocketComparisonProperty.Length, new Rocket[]
        {
            _fixture.Build<Rocket>().With(x => x.Length, (double?)null).Create(),
            _fixture.Build<Rocket>().With(x => x.Length, (double?)null).Create()
        });

        result.Should().BeApproximately(0.0, _precision);
    }

    [Theory]
    // Value [70] : 0.0 - 40, 0.25 - 50, 0.5 - [70], 0.75 - 90, 1.0 - 120
    [InlineData(ERocketComparisonProperty.Length, 0.5)] 
    // Value [200] : 0.0 - 100, 0.33 - [200], 0.67 - 300, 1.0 - 700
    [InlineData(ERocketComparisonProperty.LiftoffMass, 0.33)]
    // Value [2000] : 0.0 - 5000, 0.25 - 4000, 0.5 - 3000, 0.75 - [2000], 1.0 - 1000 (descending)
    [InlineData(ERocketComparisonProperty.LaunchCost, 0.75)] 
    public void CalculateFraction_ShouldReturnCorrectFractionForOneRocket_WhenPropertyIsValid(ERocketComparisonProperty property, double expectedResult)
    {
        var result = _calculator.CalculateFraction(property, new Rocket[] { _rockets[0] });

        result.Should().BeApproximately(expectedResult, _precision);
    }

    [Theory]
    // Value [(90 + 40) / 2 = 65] : 0.0 - 40, 0.25 - 50, 0.5 - 70, 0.75 - 90, 1.0 - 120 -> between 0.25 & 0.5 -> 0.375
    [InlineData(ERocketComparisonProperty.Length, 0.375)]
    // Value [(100 + 300) / 2 = 200] : 0.0 - 100, 0.33 - [200], 0.67 - 300, 1.0 - 700
    [InlineData(ERocketComparisonProperty.LiftoffMass, 0.33)]
    // Value [(4000 + 5000) / 2 = 4500] : 0.0 - 5000, 0.25 - 4000, 0.5 - 3000, 0.75 - [2000], 1.0 - 1000 (descending) -> between 0.0 & 0.25 -> 0.125
    [InlineData(ERocketComparisonProperty.LaunchCost, 0.125)]
    public void CalculateFraction_ShouldReturnCorrectFractionForGroupOfRockets_WhenPropertyIsValid(ERocketComparisonProperty property, double expectedResult)
    {
        var result = _calculator.CalculateFraction(property, new Rocket[] { _rockets[1], _rockets[2] });

        result.Should().BeApproximately(expectedResult, _precision);
    }
}
