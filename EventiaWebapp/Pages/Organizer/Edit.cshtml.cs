using System.ComponentModel.DataAnnotations;
using EventiaWebapp.Models;
using EventiaWebapp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EventHandler = EventiaWebapp.Services.EventHandler;

namespace EventiaWebapp.Pages.Organizer;

public class EditModel : PageModel
{
    private readonly EventHandler _eventHandler;
    private readonly UserManager<Models.User> _userManager;
    private readonly OrganizerHandler _organizerHandler;
    public Event? CurrentEvent;
    [Required] [BindProperty] public Input Input { get; set; }

    public EditModel(Services.EventHandler eventHandler, UserManager<Models.User> userManager,
        OrganizerHandler organizerHandler)
    {
        _eventHandler = eventHandler;
        _userManager = userManager;
        _organizerHandler = organizerHandler;
    }

    public async Task<IActionResult> OnGet(int? id)
    {
        if (id == null) return NotFound();

        CurrentEvent = await _eventHandler.GetEvent(id);
        Input = new Input();
        Input.FillInput(CurrentEvent);

        return Page();
    }

    public async Task<IActionResult> OnPost(int id)
    {
        if (!ModelState.IsValid) return Page();
        var organizer = await _userManager.GetUserAsync(User);

        CurrentEvent = await _eventHandler.GetEvent(id);
        bool created = await _organizerHandler.EditEvent(CurrentEvent,
            Input.Adress,
            Input.Place,
            Input.Date,
            Input.UtcOffset,
            Input.Title,
            Input.Description,
            Input.SpotsAvailable);

        if (created) return RedirectToPage("/Organizer/Index");

        return Page();
    }
}