using System.Security.Claims;
using EventiaWebapp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventiaWebapp.Pages.User;

//TODO: Användare ska kunna ta bort sig från en Event.
//TODO: Email utskick?

[Authorize(Roles = Config.ATTENDEE_ROLE_NAME)]
public class MyEventsModel : PageModel
{
    private readonly Services.EventHandler _eventHandler;
    private readonly UserManager<Models.User> _userManager;
    public Models.User? CurrentUser;
    public List<Event>? Events;

    public MyEventsModel(Services.EventHandler eventHandler, UserManager<Models.User> userManager)
    {
        _eventHandler = eventHandler;
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = _userManager.GetUserId(User);

        CurrentUser = await _eventHandler.GetUserAsync(userId);
        if (CurrentUser == null) return Page();

        Events = await _eventHandler.GetAttendeeEvents(CurrentUser);
        Events = Events.OrderBy(evt => evt.Date).ToList();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int eventId)
    {
        var userId = _userManager.GetUserId(User);

        CurrentUser = await _eventHandler.GetUserAsync(userId);
        if (CurrentUser == null) return Page();

        var thisEvent = await _eventHandler.GetEvent(eventId);
        await _eventHandler.UnRegisterToEvent(CurrentUser, thisEvent);

        Events = await _eventHandler.GetAttendeeEvents(CurrentUser);
        Events = Events.OrderBy(evt => evt.Date).ToList();

        return Page();
    }
}