using System.ComponentModel.DataAnnotations;
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

    [BindProperty]
    [DataType(DataType.Password)]
    [Display(Name = "New Password")]
    public string NewPassword { get; set; }

    [BindProperty]
    [DataType(DataType.Password)]
    [Display(Name = "Current Password")]
    public string CurrentPassword { get; set; }

    public string? ChangedPasswordMessage { get; set; }

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

    public async Task<IActionResult> OnPostAsync()
    {
        var userId = _userManager.GetUserId(User);

        CurrentUser = await _eventHandler.GetUserAsync(userId);
        if (!ModelState.IsValid) return Page();

        var result = await _userManager.ChangePasswordAsync(CurrentUser, CurrentPassword, NewPassword);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
        }

        ChangedPasswordMessage = "Password has been changed successfully";

        return Page();
    }
}