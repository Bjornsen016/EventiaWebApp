using System.Security.Claims;
using EventiaWebapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EventiaWebapp.Pages.Administrator;

[Authorize(Roles = "administrator")]
public class IndexModel : PageModel
{
    private readonly UserManager<Models.User> _userManager;
    public List<Models.User>? Users { get; set; }

    public IndexModel(UserManager<Models.User> userManager)
    {
        _userManager = userManager;
    }

    public async Task OnGetAsync()
    {
        Users = await _userManager.Users.ToListAsync();
    }

    public async Task<IActionResult> OnPostAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (await IsOrganizer(user))
        {
            await _userManager.RemoveFromRoleAsync(user, "organizer");
        }
        else
        {
            await _userManager.AddToRoleAsync(user, "organizer");
        }

        Users = await _userManager.Users.ToListAsync();
        return Page();
    }

    public async Task<bool> IsOrganizer(Models.User user)
    {
        return await _userManager.IsInRoleAsync(user, "organizer");
    }
}