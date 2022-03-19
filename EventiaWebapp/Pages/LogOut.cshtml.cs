using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventiaWebapp.Pages;

public class LogOutModel : PageModel
{
    public IActionResult OnGet()
    {
        Response.Cookies.Delete("attendee");
        return RedirectToPage("/Index");
    }
}