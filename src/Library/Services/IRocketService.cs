using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    public interface IRocketService
    {
        Task<(int, List<Rocket>)> GetRocketsAsync(int pageNumber = 1);

        Task<Rocket> GetRocketAsync(int id);
    }
}
