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
    public string SignsSort { get; set; }
    public string DateSort { get; set; }
    public List<Event> Events { get; set; }

    public IndexModel(OrganizerHandler organizerHandler, UserManager<Models.User> userManager)
    {
        _organizerHandler = organizerHandler;
        _userManager = userManager;
    }

    public async Task OnGetAsync(string sortOrder)
    {
        SignsSort = sortOrder == "signed_up" ? "signed_up_desc" : "signed_up";
        DateSort = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
        var userId = _userManager.GetUserId(User);
        var evts = await _organizerHandler.GetOrganizersEvents(userId);

        switch (sortOrder)
        {
            case "signed_up":
                Events = evts.OrderBy(e => e.Attendees.Count).ToList();
                break;
            case "signed_up_desc":
                Events = evts.OrderByDescending(e => e.Attendees.Count).ToList();
                break;
            case "date_desc":
                Events = evts.OrderByDescending(e => e.Date).ToList();
                break;
            default:
                Events = evts;
                break;
        }
    }
}