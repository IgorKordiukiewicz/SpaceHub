namespace SpaceHub.Application.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class SymbolAttribute : Attribute
{
    public string Symbol { get; set; }

    public SymbolAttribute(string symbol)
    {
        Symbol = symbol;
    }
}
