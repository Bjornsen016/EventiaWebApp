using System.Security.Claims;
using EventiaWebapp.Models;
using EventiaWebapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventiaWebapp.Pages;

[Authorize(Roles = Config.ATTENDEE_ROLE_NAME)]
public class JoinModel : PageModel
{
    public Event? CurrentEvent;
    public bool IsJoined;
    public bool EventFull;
    public Models.User? CurrentUser;
    private readonly Services.EventHandler _eventHandler;
    private readonly UserManager<Models.User> _userManager;

    public JoinModel(Services.EventHandler eventHandler, UserManager<Models.User> userManager)
    {
        _eventHandler = eventHandler;
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var userId = _userManager.GetUserId(User);

        CurrentEvent = await _eventHandler.GetEvent(id);
        CurrentUser = await _eventHandler.GetUserAsync(userId);

        if (CurrentEvent == null)
        {
            return NotFound();
        }

        var spotsLeft = CurrentEvent.SpotsAvailable - CurrentEvent.Attendees.Count;
        if (spotsLeft == 0)
        {
            EventFull = true;
        }

        IsJoined = CurrentUser.JoinedEvents.Contains(CurrentEvent);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var userId = _userManager.GetUserId(User);

        CurrentEvent = await _eventHandler.GetEvent(id);
        CurrentUser = await _eventHandler.GetUserAsync(userId);

        if (CurrentEvent == null || CurrentUser == null) return Page();

        try
        {
            IsJoined = await _eventHandler.RegisterToEvent(CurrentUser, CurrentEvent);
        }
        catch (SpotsFilledException e)
        {
            IsJoined = false;
            EventFull = true;
        }

        return Page();
    }
}