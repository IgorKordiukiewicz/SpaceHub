using SpaceHub.Contracts.Attributes;

namespace SpaceHub.Contracts.Enums;

public enum ERocketComparisonProperty
{
    [Unit("m")]
    Length,

    [Unit("m")]
    Diameter,

    [Unit("T")]
    LiftoffMass,

    [Unit("kN")]
    LiftoffThrust,

    [Unit("$")]
    CostPerKgToLeo,

    [Unit("$")]
    CostPerKgToGeo,
}