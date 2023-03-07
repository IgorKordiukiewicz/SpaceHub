using SpaceHub.Contracts.Enums;
using SpaceHub.Domain.Models;

namespace SpaceHub.Domain;

public record PropertyRanking(double? Value = null, double Fraction = 0.0, double? Rank = null);
public record PropertyTopValue(double Value, IReadOnlyList<string> Names);

public class RocketComparisonCalculator
{
    private readonly Dictionary<ERocketComparisonProperty, Property> _properties;

    public RocketComparisonCalculator(IEnumerable<Rocket> rockets)
    {
        _properties = new()
        {
            { ERocketComparisonProperty.Length, new(rockets, x => x.Length) },
            { ERocketComparisonProperty.Diameter, new(rockets, x => x.Diameter) },
            { ERocketComparisonProperty.LiftoffMass, new(rockets, x => x.LiftoffMass) },
            { ERocketComparisonProperty.LiftoffThrust, new(rockets, x => x.LiftoffThrust) },
            { ERocketComparisonProperty.CostPerKgToLeo, new(rockets, x => x.CostPerKgToLeo, true) },
            { ERocketComparisonProperty.CostPerKgToGeo, new(rockets, x => x.CostPerKgToGeo, true) },
        };
    }

    public PropertyRanking CalculatePropertyRanking(ERocketComparisonProperty property, IEnumerable<Rocket> rockets)
    {
        if(!_properties.ContainsKey(property))
        {
            return new();
        }

        return _properties[property].CalculateFractionAndRank(rockets);
    }

    public IReadOnlyDictionary<ERocketComparisonProperty, IReadOnlyList<PropertyTopValue>> GetTopRockets(int count)
    {
        var result = new Dictionary<ERocketComparisonProperty, IReadOnlyList<PropertyTopValue>>();
        foreach (var (propertyType, property) in _properties)
        {
            result.Add(propertyType, property.GetTopRockets(count));
        }
        return result;
    }

    private class Property
    {
        private readonly Func<Rocket, double?> _property;
        private readonly bool _descending;
        private readonly List<long> _valuesRanked;
        private readonly double _rankMultiplier;
        private readonly Dictionary<long, IEnumerable<string>> _namesByValue;

        public Property(IEnumerable<Rocket> rockets, Func<Rocket, double?> property, bool descending = false)
        {
            _property = property;
            _descending = descending;

            // TODO: Improve performance?
            var valuesRanked = rockets.Select(x => (Value: RealValueToCalculationValue(property(x).GetValueOrDefault()), Name: x.Name))
                .Where(x => x.Value > 0)
                .GroupBy(x => x.Value);
            valuesRanked = descending
                ? valuesRanked.OrderBy(x => x.Key)
                : valuesRanked.OrderByDescending(x => x.Key);

            _valuesRanked = valuesRanked.Select(x => x.Key).ToList();
            _namesByValue = valuesRanked.ToDictionary(k => k.Key, v => v.Select(x => x.Name));

            _rankMultiplier = 1.0 / (_valuesRanked.Count - 1);
        }

        public List<PropertyTopValue> GetTopRockets(int count)
        {
            var result = new List<PropertyTopValue>();
            for(int i = 0; i < count; ++i)
            {
                if(i >= _valuesRanked.Count)
                {
                    break;
                }

                var value = _valuesRanked[i];
                result.Add(new(CalculationValueToRealValue(value), _namesByValue[value].Distinct().ToList()));
            }
            return result;
        }

        public PropertyRanking CalculateFractionAndRank(IEnumerable<Rocket> rockets)
        {
            // TODO: Is calculating average of a group the correct approach?
            var avg = rockets.Average(_property);
            if (avg is null)
            {
                return new();
            }

            var rankIndex = CalculateRankIndex(RealValueToCalculationValue(avg.Value));
            return new(avg, CalculateFraction(rankIndex), rankIndex + 1);
        }

        private double CalculateFraction(double rankIndex)
        {
            return 1.0 - (_rankMultiplier * rankIndex);
        }

        private double CalculateRankIndex(long value)
        {
            // TODO: Improve performance?
            for (int i = 0; i < _valuesRanked.Count; ++i)
            {
                if (value == _valuesRanked[i])
                {
                    return i;
                }

                if(i + 1 < _valuesRanked.Count)
                {
                    if((_descending && value > _valuesRanked[i] && value < _valuesRanked[i + 1])
                        || (!_descending && value < _valuesRanked[i] && value > _valuesRanked[i + 1]))
                    {
                        return i + 0.5;
                    }
                }
            }

            return 0.0;
        }

        private static long RealValueToCalculationValue(double value)
            => (long)(value * 10);

        private static double CalculationValueToRealValue(long value)
            => value / 10.0;
    }
}
