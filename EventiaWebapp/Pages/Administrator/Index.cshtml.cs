using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EventiaWebapp.Pages.Administrator;

//TODO: Söka på användare för att lättare hitta en specifik användare.

[Authorize(Roles = Config.ADMIN_ROLE_NAME)]
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
            await _userManager.RemoveFromRoleAsync(user, Config.ORGANIZER_ROLE_NAME);
        }
        else
        {
            await _userManager.AddToRoleAsync(user, Config.ORGANIZER_ROLE_NAME);
        }

        Users = await _userManager.Users.ToListAsync();
        return Page();
    }

    public async Task<bool> IsOrganizer(Models.User user)
    {
        return await _userManager.IsInRoleAsync(user, Config.ORGANIZER_ROLE_NAME);
    }
}