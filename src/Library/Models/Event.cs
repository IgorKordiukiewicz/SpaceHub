using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public record Event
    {
        public int ApiId { get; init; }
        public string Name { get; init; }
        public string Type { get; init; }
        public string Description { get; init; }
        public string Location { get; init; }
        public string? NewsUrl { get; init; }
        public string? VideoUrl { get; init; }
        public string ImageUrl { get; init; }
        public DateTime? Date { get; init; }
    }
}
