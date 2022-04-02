using System.ComponentModel.DataAnnotations;
using EventiaWebapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventiaWebapp.Pages.Organizer;

//TODO: Länka till en bild, kanske till och med ladda upp en bild för eventet.

[Authorize(Roles = Config.ORGANIZER_ROLE_NAME)]
public class AddEventModel : PageModel
{
    private readonly UserManager<Models.User> _userManager;
    private readonly OrganizerHandler _organizerHandler;
    [Required] [BindProperty] public Input Input { get; set; }

    public AddEventModel(UserManager<Models.User> userManager, OrganizerHandler organizerHandler)
    {
        _userManager = userManager;
        _organizerHandler = organizerHandler;
    }


    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid) return Page();
        var organizer = await _userManager.GetUserAsync(User);

        bool created = await _organizerHandler.CreateNewEvent(
            Input.Adress,
            Input.Place,
            organizer,
            Input.Date,
            Input.UtcOffset,
            Input.Title,
            Input.Description,
            Input.SpotsAvailable);

        if (created) return RedirectToPage("/Organizer/Index");

        return Page();
    }
}

public class Input
{
    [Required]
    [DataType(DataType.MultilineText)]
    public string Adress { get; set; }

    [Required] [DataType(DataType.Text)] public string Place { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime Date { get; set; }

    [Required] public int UtcOffset { get; set; }

    [Required] [DataType(DataType.Text)] public string Title { get; set; }

    [Required]
    [DataType(DataType.MultilineText)]
    public string Description { get; set; }

    [Required]
    [Display(Name = "Spots Available")]
    public int SpotsAvailable { get; set; }
}