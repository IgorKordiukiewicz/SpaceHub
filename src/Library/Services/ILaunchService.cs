using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    public interface ILaunchService
    {
        Task<List<Launch>> GetUpcomingLaunchesAsync();

        Task<List<Launch>> GetPreviousLaunchesAsync();

        Task<Launch> GetLaunchAsync(string id);
    }
}
