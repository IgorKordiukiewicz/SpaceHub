using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Utils
{
    public class Pagination
    {
        public int ItemsPerPage { get; set; } = 10;

        public int GetOffset(int pageNumber)
        {
            return (pageNumber - 1) * ItemsPerPage;
        }

        public int GetPagesCount(int itemsCount)
        {
            return (itemsCount - 1) / ItemsPerPage + 1;
        }
    }
}
