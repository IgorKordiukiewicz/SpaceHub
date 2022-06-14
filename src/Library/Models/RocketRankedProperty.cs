using Library.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class RocketRankedProperty
    {
        public Rocket Rocket { get; init; }
        public RocketRankedPropertyType Type { get; init; }
        public object? Value { get; init; }
        public object? SecondaryValue { get; init; }
    }
}
