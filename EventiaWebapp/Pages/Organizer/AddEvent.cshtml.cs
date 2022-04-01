using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventiaWebapp.Pages.Organizer;

[Authorize(Roles = Config.ORGANIZER_ROLE_NAME)]
public class AddEventModel : PageModel
{
    public void OnGet()
    {
    }
}