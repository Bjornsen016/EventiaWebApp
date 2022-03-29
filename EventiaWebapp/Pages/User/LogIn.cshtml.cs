using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventiaWebapp.Pages.User;

[AllowAnonymous]
public class LogInModel : PageModel
{
    private readonly SignInManager<Models.User> _signInManager;
    public string ReturnUrl { get; set; }

    [EmailAddress] [BindProperty] public string Email { get; set; }
    [BindProperty] public string Password { get; set; }

    public LogInModel(SignInManager<Models.User> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task OnGetAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        if (!ModelState.IsValid) return Page();

        var result = await _signInManager.PasswordSignInAsync(Email, Password, true, false);

        if (result.Succeeded) return LocalRedirect(returnUrl);

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return Page();
    }
}