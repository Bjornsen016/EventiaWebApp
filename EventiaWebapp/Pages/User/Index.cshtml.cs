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
    public Models.User User;

    public IndexModel()
    {
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var id = base.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var userId = int.Parse(id);


        return Page();
    }
}