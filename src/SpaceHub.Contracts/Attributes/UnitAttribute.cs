namespace SpaceHub.Contracts.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class UnitAttribute : Attribute
{
    public string Unit { get; set; }

    public UnitAttribute(string unit)
    {
        Unit = unit;
    }
}
