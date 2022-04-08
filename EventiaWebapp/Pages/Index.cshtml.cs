using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventiaWebapp.Pages;

[AllowAnonymous]
public class IndexModel : PageModel
{
    private readonly Services.EventHandler _eventHandler;
    private readonly UserManager<Models.User> _userManager;
    private readonly SignInManager<Models.User> _signInManager;
    public Models.User? CurrentUser;
    public bool IsAdmin;
    public List<Models.User> Attendees;
    public List<Models.User> Organizers;
    public List<Models.Event> AllEvents;

    public IndexModel(Services.EventHandler eventHandler, UserManager<Models.User> userManager,
        SignInManager<Models.User> signInManager)
    {
        _eventHandler = eventHandler;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (!_signInManager.IsSignedIn(User)) return Page();

        var userId = _userManager.GetUserId(User);
        CurrentUser = await _eventHandler.GetUserAsync(userId);

        IsAdmin = await _userManager.IsInRoleAsync(CurrentUser, Config.ADMIN_ROLE_NAME);

        if (!IsAdmin) return Page();

        var users = await _userManager.GetUsersInRoleAsync(Config.ATTENDEE_ROLE_NAME);
        Attendees = users.ToList();

        var organizers = await _userManager.GetUsersInRoleAsync(Config.ORGANIZER_ROLE_NAME);
        Organizers = organizers.ToList();

        AllEvents = await _eventHandler.GetEventsAsync();

        return Page();
    }
}