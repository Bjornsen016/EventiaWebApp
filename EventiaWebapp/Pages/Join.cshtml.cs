using EventiaWebapp.Data;
using EventiaWebapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventiaWebapp.Pages;

public class JoinModel : PageModel
{
    public Event? CurrentEvent;
    public bool IsJoined;
    public Attendee Attendee;
    private readonly Services.EventHandler _eventHandler;

    public JoinModel(Services.EventHandler eventHandler)
    {
        _eventHandler = eventHandler;
    }

    public async Task<IActionResult> OnGetAsync(int? id, int userId)
    {
        if (id == null)
        {
            return NotFound();
        }

        CurrentEvent = await _eventHandler.GetEvent(id);
        Attendee = await _eventHandler.GetAttendeeAsync(1);

        if (CurrentEvent == null)
        {
            return NotFound();
        }

        if (Attendee == null)
        {
            return NotFound("You must log in.");
        }

        IsJoined = Attendee.Events.Contains(CurrentEvent);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id /*, int userId*/)
    {
        CurrentEvent = await _eventHandler.GetEvent(id);
        Attendee = await _eventHandler.GetAttendeeAsync(1);
        if (CurrentEvent != null && Attendee != null)
            IsJoined = await _eventHandler.RegisterToEvent(Attendee, CurrentEvent);

        return Page();
    }
}