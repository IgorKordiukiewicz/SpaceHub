using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Api.Responses;
using Refit;

namespace Library.Api
{
    public interface ILaunchApi
    {
        [Get("/launch/upcoming/")]
        Task<LaunchesResponse> GetUpcomingLaunches();

        [Get("/launch/upcoming/{id}")]
        Task<LaunchDetailResponse> GetUpcomingLaunch(string id);
    }
}
