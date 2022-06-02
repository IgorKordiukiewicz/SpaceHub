using Library.Api.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    public interface IRocketService
    {
        Task<List<RocketConfigResponse>> GetRockets();
    }
}
