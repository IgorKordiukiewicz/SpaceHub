using Library.Models;
using Library.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    public interface ILaunchService
    {
        Task<(int, List<Launch>)> GetUpcomingLaunchesAsync(string? searchValue, int pageNumber = 1, int itemsPerPage = 12);

        Task<(int, List<Launch>)> GetPreviousLaunchesAsync(string? searchValue, int pageNumber = 1, int itemsPerPage = 12);

        Task<Launch> GetLaunchAsync(string id);
    }
}
