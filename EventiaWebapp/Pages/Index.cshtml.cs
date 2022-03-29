using System.Security.Claims;
using EventiaWebapp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EventiaWebapp.Pages;

[AllowAnonymous]
public class IndexModel : PageModel
{
    private readonly Services.EventHandler _eventHandler;
    private readonly UserManager<Models.User> _userManager;
    private readonly SignInManager<Models.User> _signInManager;
    public Models.User? CurrentUser;

    public IndexModel(Services.EventHandler eventHandler, UserManager<Models.User> userManager,
        SignInManager<Models.User> signInManager)
    {
        _eventHandler = eventHandler;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (!_signInManager.IsSignedIn(User)) return Page();

        var userId = _userManager.GetUserId(User);
        CurrentUser = await _eventHandler.GetUserAsync(userId);

        return Page();
    }
}