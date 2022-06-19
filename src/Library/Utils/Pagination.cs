using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Utils
{
    public static class Pagination
    {
        public static int GetOffset(int pageNumber, int itemsPerPage)
        {
            return (pageNumber - 1) * itemsPerPage;
        }

        public static int GetPagesCount(int itemsCount, int itemsPerPage)
        {
            return (itemsCount - 1) / itemsPerPage + 1;
        }
    }
}
