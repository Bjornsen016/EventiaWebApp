using System.Security.Claims;
using EventiaWebapp.Models;
using EventiaWebapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventiaWebapp.Pages;

[Authorize]
public class JoinModel : PageModel
{
    public Event? CurrentEvent;
    public bool IsJoined;
    public bool EventFull;
    public Models.User User;
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

        var userIdString = base.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var userId = int.Parse(userIdString);

        CurrentEvent = await _eventHandler.GetEvent(id);
        User = await _eventHandler.GetAttendeeAsync(userId);

        if (CurrentEvent == null)
        {
            return NotFound();
        }

        var spotsLeft = CurrentEvent.SpotsAvailable - CurrentEvent.Attendees.Count;
        if (spotsLeft == 0)
        {
            EventFull = true;
        }

        IsJoined = User.Events.Contains(CurrentEvent);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var userIdString = base.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var userId = int.Parse(userIdString);

        CurrentEvent = await _eventHandler.GetEvent(id);
        User = await _eventHandler.GetAttendeeAsync(userId);

        if (CurrentEvent == null || User == null) return Page();

        try
        {
            IsJoined = await _eventHandler.RegisterToEvent(User, CurrentEvent);
        }
        catch (SpotsFilledException e)
        {
            IsJoined = false;
            EventFull = true;
        }

        return Page();
    }
}