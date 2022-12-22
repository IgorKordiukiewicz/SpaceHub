namespace Application.Common;

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
