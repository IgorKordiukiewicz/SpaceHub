using Library.Api.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Api
{
    public interface IApiClient
    {
        Task<List<ArticleResponse>> GetArticlesAsync();
    }
}
