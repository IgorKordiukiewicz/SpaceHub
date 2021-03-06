using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Library.Api.Responses
{
    public record EventsResponse : MultiElementResponse
    {
        [JsonPropertyName("results")]
        public IEnumerable<EventResponse> Events { get; init; }
    }
}
