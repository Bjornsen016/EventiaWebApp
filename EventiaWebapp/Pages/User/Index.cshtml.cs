using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EventHandler = EventiaWebapp.Services.EventHandler;

namespace EventiaWebapp.Pages.User;

//TODO: Skiv ut informationen bättre och med mer info.

[Authorize]
public class IndexModel : PageModel
{
    private readonly UserManager<Models.User> _userManager;
    private readonly EventHandler _eventHandler;
    public Models.User? CurrentUser;

    public IndexModel(UserManager<Models.User> userManager, Services.EventHandler eventHandler)
    {
        _userManager = userManager;
        _eventHandler = eventHandler;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = _userManager.GetUserId(User);

        CurrentUser = await _eventHandler.GetUserAsync(userId);

        return Page();
    }
}