using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventiaWebapp.Pages;

[AllowAnonymous]
public class LogOutModel : PageModel
{
    public async Task<IActionResult> OnGetAsync(string? returnUrl = null)
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (returnUrl != null)
        {
            return LocalRedirect(returnUrl);
        }

        return RedirectToPage("/Index");
    }
}