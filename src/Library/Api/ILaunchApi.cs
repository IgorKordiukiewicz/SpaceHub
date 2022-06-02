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

        [Get("/launch/previous/")]
        Task<LaunchesResponse> GetPreviousLaunches();

        [Get("/launch/{id}")]
        Task<LaunchDetailResponse> GetLaunch(string id);

        [Get("/config/launcher")]
        Task<RocketsResponse> GetRockets();
    }
}
