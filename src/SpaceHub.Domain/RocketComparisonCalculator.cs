using SpaceHub.Contracts.Enums;
using SpaceHub.Domain.Models;

namespace SpaceHub.Domain;

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
            { ERocketComparisonProperty.SuccessfulLaunches, new(rockets, x => x.SuccessfulLaunches) },
        };
    }

    public double CalculateFraction(ERocketComparisonProperty property, IEnumerable<Rocket> rockets)
    {
        if(!_properties.ContainsKey(property))
        {
            return 0.0;
        }

        return _properties[property].CalculateFraction(rockets);
    }

    private class Property
    {
        private readonly Func<Rocket, double?> _property;
        private readonly bool _descending;
        private readonly List<long> _valuesRanked;
        private readonly double _rankMultiplier;

        public Property(IEnumerable<Rocket> rockets, Func<Rocket, double?> property, bool descending = false)
        {
            _property = property;
            _descending = descending;

            // TODO: Improve performance?
            _valuesRanked = rockets.Select(x => ConvertProperty(property(x).GetValueOrDefault()))
                .Where(x => x > 0)
                .GroupBy(x => x)
                .Select(x => x.Key)
                .Order()
                .ToList();

            _rankMultiplier = 1.0 / (_valuesRanked.Count - 1);
        }

        public double CalculateFraction(IEnumerable<Rocket> rockets)
        {
            // TODO: Is calculating average of a group the correct approach?
            var avg = rockets.Average(_property);
            if (avg is null)
            {
                return 0.0;
            }

            var rank = CalculateRank(ConvertProperty(avg.Value));
            var rankFraction = _rankMultiplier * rank;
            return _descending ? (1.0 - rankFraction) : rankFraction;
        }

        private double CalculateRank(long value)
        {
            // TODO: Improve performance?
            for (int i = 0; i < _valuesRanked.Count; ++i)
            {
                if (value == _valuesRanked[i])
                {
                    return i;
                }

                if (i + 1 < _valuesRanked.Count && value > _valuesRanked[i] && value < _valuesRanked[i + 1])
                {
                    return i + 0.5;
                }
            }

            return 0.0;
        }

        private static long ConvertProperty(double property)
            => (long)property * 10;
    }
}
