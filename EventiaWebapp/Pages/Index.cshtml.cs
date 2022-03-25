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
    public Attendee? CurrentUser;

    public IndexModel(Services.EventHandler eventHandler)
    {
        _eventHandler = eventHandler;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (!User.Identity.IsAuthenticated) return Page();

        var id = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var userId = int.Parse(id);

        CurrentUser = await _eventHandler.GetAttendeeAsync(userId);

        return Page();
    }
}