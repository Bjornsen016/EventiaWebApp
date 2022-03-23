using System.Security.Claims;
using EventiaWebapp.Models;
using EventiaWebapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventiaWebapp.Pages.User;

[Authorize]
public class IndexModel : PageModel
{
    private readonly LoginHandler _loginHandler;
    public Attendee Attendee;

    public IndexModel(LoginHandler loginHandler)
    {
        _loginHandler = loginHandler;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var id = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var userId = int.Parse(id);

        Attendee = await _loginHandler.GetAttendee(userId);
        return Page();
    }
}