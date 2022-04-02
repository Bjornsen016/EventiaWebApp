using System.Security.Claims;
using EventiaWebapp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventiaWebapp.Pages.User;

//TODO: Användare ska kunna ta bort sig från en Event.
//TODO: Email utskick?

[Authorize(Roles = Config.ATTENDEE_ROLE_NAME)]
public class MyEventsModel : PageModel
{
    private readonly Services.EventHandler _eventHandler;
    public Models.User? CurrentUser;
    public List<Event>? Events;

    public MyEventsModel(Services.EventHandler eventHandler)
    {
        _eventHandler = eventHandler;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        CurrentUser = await _eventHandler.GetUserAsync(userId);
        if (CurrentUser == null) return Page();

        Events = await _eventHandler.GetAttendeeEvents(CurrentUser);
        Events = Events.OrderBy(evt => evt.Date).ToList();

        return Page();
    }
}