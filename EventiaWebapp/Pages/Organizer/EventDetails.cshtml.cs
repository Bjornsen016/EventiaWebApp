using EventiaWebapp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EventHandler = EventiaWebapp.Services.EventHandler;

namespace EventiaWebapp.Pages.Organizer;

public class EventDetailsModel : PageModel
{
    private readonly EventHandler _eventHandler;
    public Event CurrentEvent { get; set; }

    public EventDetailsModel(EventHandler eventHandler)
    {
        _eventHandler = eventHandler;
    }

    public async Task OnGet(int id)
    {
        CurrentEvent = await _eventHandler.GetEvent(id);
    }
}