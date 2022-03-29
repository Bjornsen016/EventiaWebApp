using System.Security.Claims;
using EventiaWebapp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventiaWebapp.Pages;

[AllowAnonymous]
public class IndexModel : PageModel
{
    private readonly Services.EventHandler _eventHandler;
    public Models.User? CurrentUser;

    public IndexModel(Services.EventHandler eventHandler)
    {
        _eventHandler = eventHandler;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (!User.Identity.IsAuthenticated) return Page();

        var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        CurrentUser = await _eventHandler.GetUserAsync(userId);

        return Page();
    }
}