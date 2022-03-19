using EventiaWebapp.Data;
using EventiaWebapp.Models;
using EventiaWebapp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventiaWebapp.Pages;

public class JoinModel : PageModel
{
    public Event? CurrentEvent;
    public bool IsJoined;
    public bool EventFull;
    public Attendee Attendee;
    private readonly Services.EventHandler _eventHandler;

    public JoinModel(Services.EventHandler eventHandler)
    {
        _eventHandler = eventHandler;
    }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var userIdString = Request.Cookies["attendee"];
        if (userIdString == null) return RedirectToPage("/LogIn");

        var userId = int.Parse(userIdString);

        CurrentEvent = await _eventHandler.GetEvent(id);
        Attendee = await _eventHandler.GetAttendeeAsync(userId);

        if (CurrentEvent == null)
        {
            return NotFound();
        }

        var spotsLeft = CurrentEvent.SpotsAvailable - CurrentEvent.Attendees.Count;
        if (spotsLeft == 0)
        {
            EventFull = true;
        }

        IsJoined = Attendee.Events.Contains(CurrentEvent);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var userIdString = Request.Cookies["attendee"];
        if (userIdString == null) return RedirectToPage("/LogIn");

        var userId = int.Parse(userIdString);

        CurrentEvent = await _eventHandler.GetEvent(id);
        Attendee = await _eventHandler.GetAttendeeAsync(userId);

        if (CurrentEvent == null || Attendee == null) return Page();

        try
        {
            IsJoined = await _eventHandler.RegisterToEvent(Attendee, CurrentEvent);
        }
        catch (SpotsFilledException e)
        {
            IsJoined = false;
            EventFull = true;
        }

        return Page();
    }
}