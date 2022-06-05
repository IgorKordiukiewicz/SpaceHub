using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public record Program
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public List<Agency> Agencies { get; init; }
        public string ImageUrl { get; init; }
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public string? InfoUrl { get; init; }
        public string? WikiUrl { get; init; }
    }
}
