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
        public Pagination Pagination { get; }

        Task<(int, List<Launch>)> GetUpcomingLaunchesAsync(string? searchValue, int pageNumber = 1);

        Task<(int, List<Launch>)> GetPreviousLaunchesAsync(string? searchValue, int pageNumber = 1);

        Task<Launch> GetLaunchAsync(string id);
    }
}
