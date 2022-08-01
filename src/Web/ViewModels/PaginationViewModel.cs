namespace Web.ViewModels;

public class PaginationViewModel
{
    public int PageNumber { get; set; }
    public int PagesCount { get; set; }
    public string PageUrl { get; set; }
    public Dictionary<string, string> Parameters { get; set; }

    public PaginationViewModel(int pageNumber, int pagesCount, string pageUrl, Dictionary<string, string>? parameters = null)
    {
        PageNumber = pageNumber;
        PagesCount = pagesCount;
        PageUrl = pageUrl;

        if(parameters != null)
        {
            parameters.Add("pageNumber", pageNumber.ToString());
        }
        else
        {
            parameters = new Dictionary<string, string>()
            {
                {"pageNumber", pageNumber.ToString() }
            };
        }
        Parameters = parameters;
    }
}
