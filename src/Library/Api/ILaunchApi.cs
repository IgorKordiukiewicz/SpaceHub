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
        Task<LaunchesResponse> GetUpcomingLaunchesAsync();

        [Get("/launch/previous/")]
        Task<LaunchesResponse> GetPreviousLaunchesAsync();

        [Get("/launch/{id}")]
        Task<LaunchDetailResponse> GetLaunchAsync(string id);

        [Get("/config/launcher/?limit={limit}&offset={offset}")]
        Task<RocketsResponse> GetRocketsAsync(int limit = 50, int offset = 0);

        [Get("/config/launcher/{id}/")]
        Task<RocketConfigDetailResponse> GetRocketAsync(int id);
    }
}
