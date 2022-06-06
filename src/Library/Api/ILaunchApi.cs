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
        [Get("/launch/upcoming/?limit={limit}&offset={offset}")]
        Task<LaunchesResponse> GetUpcomingLaunchesAsync(int limit, int offset = 0);

        [Get("/launch/previous/?limit={limit}&offset={offset}")]
        Task<LaunchesResponse> GetPreviousLaunchesAsync(int limit, int offset = 0);

        [Get("/launch/{id}")]
        Task<LaunchDetailResponse> GetLaunchAsync(string id);

        [Get("/config/launcher/?limit={limit}&offset={offset}")]
        Task<RocketsResponse> GetRocketsAsync(int limit = 50, int offset = 0);

        [Get("/config/launcher/?mode=detailed&limit={limit}&offset={offset}")]
        Task<RocketsDetailResponse> GetRocketsDetailAsync(int limit = 50, int offset = 0);

        [Get("/config/launcher/{id}/")]
        Task<RocketConfigDetailResponse> GetRocketAsync(int id);
    }
}
