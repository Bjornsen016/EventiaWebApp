using EventiaWebapp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventiaWebapp.Pages;

public class EventsModel : PageModel
{
    private readonly Services.EventHandler _eventHandler;
    public List<Event>? Events;

    public EventsModel(Services.EventHandler eventHandler)
    {
        _eventHandler = eventHandler;
    }

    public async Task OnGetAsync()
    {
        Events = await _eventHandler.GetEventsAsync();
    }
}