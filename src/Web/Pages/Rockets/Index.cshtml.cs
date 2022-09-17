using Library.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.ViewModels;
using Web.Mapping;

namespace Web.Pages.Rockets;

public class IndexModel : PageModel
{
    private readonly IRocketService _rocketService;

    public List<RocketIndexCardViewModel>? Rockets { get; set; }

    public PaginationViewModel Pagination { get; set; }

    [BindProperty]
    public string? SearchValue { get; set; }

    public IndexModel(IRocketService rocketService)
    {
        _rocketService = rocketService;
    }

    public async Task OnGet(string? searchValue, int pageNumber = 1)
    {
        SearchValue = searchValue;
        
        var (pagesCount, result) = await _rocketService.GetRocketsAsync(SearchValue, pageNumber);

        Rockets = result?.Select(r => r.ToRocketIndexCardViewModel()).ToList();

        Pagination = new PaginationViewModel(pageNumber, pagesCount, "/Rockets/Index", searchValue != null ? new() { { "searchValue", searchValue } } : null);
    }

    public IActionResult OnPost()
    {
        return RedirectToPage("Index", new { searchValue = SearchValue, pageNumber = 1 });
    }
}
