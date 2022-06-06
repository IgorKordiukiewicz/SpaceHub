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
        Task<(int, List<Launch>)> GetUpcomingLaunchesAsync(string? searchValue, int pageNumber = 1);

        Task<(int, List<Launch>)> GetPreviousLaunchesAsync(string? searchValue, int pageNumber = 1);

        Task<Launch> GetLaunchAsync(string id);
    }
}
