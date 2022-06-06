using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Enums
{
    public enum LaunchDateType
    {
        Upcoming,
        Previous
    }

    public enum RocketRankedPropertyType
    {
        Length,
        Diameter,
        LaunchCost,
        LiftoffMass,
        LiftoffThrust,
        LeoCapacity,
        GeoCapacity,
        CostPerKgToLeo,
        CostPerKgToGeo,
    }
}
