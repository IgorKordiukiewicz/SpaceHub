﻿using Library.Api;
using Library.Api.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    public class LaunchService : ILaunchService
    {
        private readonly ILaunchApi _launchApi;

        public LaunchService(ILaunchApi launchApi)
        {
            _launchApi = launchApi;
        }
        
        public async Task<List<LaunchResponse>> GetUpcomingLaunchesAsync()
        {
            var result = await _launchApi.GetUpcomingLaunches();
            return result.Launches.ToList();
        }

        public async Task<List<LaunchResponse>> GetPreviousLaunchesAsync()
        {
            var result = await _launchApi.GetPreviousLaunches();
            return result.Launches.ToList();
        }

        public async Task<LaunchDetailResponse> GatLaunchAsync(string id)
        {
            var result = await _launchApi.GetLaunch(id);
            return result;
        }
    }
}
