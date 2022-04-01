using EventiaWebapp.Models;
using EventiaWebapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventiaWebapp.Pages.Organizer;

[Authorize(Roles = Config.ORGANIZER_ROLE_NAME)]
public class IndexModel : PageModel
{
    private readonly OrganizerHandler _organizerHandler;
    private readonly UserManager<Models.User> _userManager;
    public List<Event> Events { get; set; }

    public IndexModel(OrganizerHandler organizerHandler, UserManager<Models.User> userManager)
    {
        _organizerHandler = organizerHandler;
        _userManager = userManager;
    }

    public async Task OnGetAsync()
    {
        var userId = _userManager.GetUserId(User);
        Events = await _organizerHandler.GetOrganizersEvents(userId);
    }
}