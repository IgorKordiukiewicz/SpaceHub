using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Library.Api.Responses
{
    public record MultiElementResponse
    {
        [JsonPropertyName("count")]
        public int Count { get; init; }
    }
}
