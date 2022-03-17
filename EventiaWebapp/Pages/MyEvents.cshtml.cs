using EventiaWebapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventiaWebapp.Pages;

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
        var userIdString = Request.Cookies["attendee"];
        if (userIdString == null) return RedirectToPage("/LogIn");

        var userId = int.Parse(userIdString);

        CurrentUser = await _eventHandler.GetAttendeeAsync(userId);
        if (CurrentUser == null) return Page();

        Events = await _eventHandler.GetAttendeeEvents(CurrentUser);
        Events = Events.OrderBy(evt => evt.Date).ToList();

        return Page();
    }
}