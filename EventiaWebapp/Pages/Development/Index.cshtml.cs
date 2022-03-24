using System.Security.Claims;
using EventiaWebapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventiaWebapp.Pages.Development;

[Authorize]
public class IndexModel : PageModel
{
    private readonly Database _database;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly LoginHandler _loginHandler;
    public bool DatabaseReseted;

    public IndexModel(Database database, IWebHostEnvironment hostingEnvironment, LoginHandler loginHandler)
    {
        _database = database;
        _hostingEnvironment = hostingEnvironment;
        _loginHandler = loginHandler;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (!_hostingEnvironment.IsDevelopment()) return NotFound();

        var id = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var userId = int.Parse(id);
        var attendee = await _loginHandler.GetAttendee(userId);

        if (!attendee.IsAdmin()) return NotFound();

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