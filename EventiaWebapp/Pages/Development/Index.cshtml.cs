using EventiaWebapp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventiaWebapp.Pages.Development;

public class IndexModel : PageModel
{
    private readonly Database _database;
    private readonly IWebHostEnvironment _hostingEnvironment;
    public bool DatabaseReseted;

    public IndexModel(Database database, IWebHostEnvironment hostingEnvironment)
    {
        _database = database;
        _hostingEnvironment = hostingEnvironment;
    }

    public IActionResult OnGet()
    {
        if (!_hostingEnvironment.IsDevelopment()) return NotFound();

        return Page();
    }

    public IActionResult OnPost()
    {
        if (!_hostingEnvironment.IsDevelopment()) return NotFound();

        _database.RecreateAndSeed();
        DatabaseReseted = true;
        return Page();
    }
}