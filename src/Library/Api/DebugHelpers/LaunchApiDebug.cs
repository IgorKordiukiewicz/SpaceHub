using Library.Api.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Library.Api.Debug
{
    public class LaunchApiDebug : ILaunchApi
    {
        public async Task<LaunchesResponse> GetUpcomingLaunches()
        {
            string workingDir = Environment.CurrentDirectory;
            string projectDir = Directory.GetParent(workingDir).Parent.FullName;
            string jsonString = new StreamReader(projectDir + "/src/Library/Api/DebugHelpers/launchResponseExample.json").ReadToEnd();

            var result = JsonSerializer.Deserialize<LaunchesResponse>(jsonString);
            return result;
        }
    }
}
