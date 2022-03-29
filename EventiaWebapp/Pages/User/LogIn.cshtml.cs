using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using EventiaWebapp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventiaWebapp.Pages;

[AllowAnonymous]
public class LogInModel : PageModel
{
    private readonly LoginHandler _loginHandler;
    public string LoginFailedMessage { get; set; }
    public string ReturnUrl { get; set; }

    [EmailAddress] [BindProperty] public string Email { get; set; }
    [BindProperty] public string Password { get; set; }

    public LogInModel(LoginHandler loginHandler)
    {
        _loginHandler = loginHandler;
    }

    public async Task<IActionResult> OnGetAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        ReturnUrl = returnUrl;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        if (!ModelState.IsValid) return Page();
        try
        {
            var user = await _loginHandler.TryLoginAttendee(Email, Password);

            var principal = _loginHandler.CreateUserCookies(user);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties {IsPersistent = true});


            return LocalRedirect(returnUrl);
        }
        catch (LoginException ex)
        {
            LoginFailedMessage = "Password is wrong.";
        }

        return Page();
    }
}