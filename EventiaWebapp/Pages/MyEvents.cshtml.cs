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

    public async Task<IActionResult> OnGetAsync(int? userId)
    {
        /* if (userId == null)
         {
             return Page();
         }*/

        CurrentUser = await _eventHandler.GetAttendeeAsync(1);
        if (CurrentUser != null)
        {
            Events = await _eventHandler.GetAttendeeEvents(CurrentUser);
            Events = Events.OrderBy(evt => evt.Date).ToList();
        }

        return Page();
    }
}