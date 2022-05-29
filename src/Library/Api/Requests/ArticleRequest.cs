using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Api.Requests
{
    public record ArticleRequest
    {
        public string? SearchValue { get; init; }
        public int PageNumber { get; init; } = 1;
    }
}
