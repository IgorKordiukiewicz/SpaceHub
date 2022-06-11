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
        [Get("/launch/upcoming/?search={searchValue}&limit={limit}&offset={offset}")]
        Task<LaunchesResponse> GetUpcomingLaunchesAsync(string? searchValue, int limit, int offset = 0);

        [Get("/launch/previous/?search={searchValue}&limit={limit}&offset={offset}")]
        Task<LaunchesResponse> GetPreviousLaunchesAsync(string? searchValue, int limit, int offset = 0);

        [Get("/launch/{id}")]
        Task<LaunchDetailResponse> GetLaunchAsync(string id);

        [Get("/config/launcher/?search={searchValue}&limit={limit}&offset={offset}")]
        Task<RocketsResponse> GetRocketsAsync(string? searchValue, int limit, int offset = 0);

        [Get("/config/launcher/?mode=detailed&limit={limit}&offset={offset}")]
        Task<RocketsDetailResponse> GetRocketsDetailAsync(int limit, int offset = 0);

        [Get("/config/launcher/{id}/")]
        Task<RocketConfigDetailResponse> GetRocketAsync(int id);

        [Get("/event/upcoming/?search={searchValue}&limit={limit}&offset={offset}")]
        Task<EventsResponse> GetUpcomingEventsAsync(string? searchValue, int limit, int offset = 0);
    }
}
