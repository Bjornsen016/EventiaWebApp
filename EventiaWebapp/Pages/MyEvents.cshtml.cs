using System.Security.Claims;
using EventiaWebapp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventiaWebapp.Pages;

[Authorize]
public class MyEventsModel : PageModel
{
    private readonly Services.EventHandler _eventHandler;
    public Attendee? CurrentUser;
    public List<Event>? Events;

    public MyEventsModel(Services.EventHandler eventHandler)
    {
        _eventHandler = eventHandler;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var id = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var userId = int.Parse(id);

        CurrentUser = await _eventHandler.GetAttendeeAsync(userId);
        if (CurrentUser == null) return Page();

        Events = await _eventHandler.GetAttendeeEvents(CurrentUser);
        Events = Events.OrderBy(evt => evt.Date).ToList();

        return Page();
    }
}