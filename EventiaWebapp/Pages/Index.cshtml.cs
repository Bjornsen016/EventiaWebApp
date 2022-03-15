using EventiaWebapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventiaWebapp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly Services.EventHandler _eventHandler;
        public Attendee? CurrentUser;

        public IndexModel(ILogger<IndexModel> logger, Services.EventHandler eventHandler)
        {
            _logger = logger;
            _eventHandler = eventHandler;
        }

        public async Task<IActionResult> OnGetAsync(int? userId)
        {
            if (userId == null) return Page();

            CurrentUser = await _eventHandler.GetAttendeeAsync(1);
            //TODO: Dubbelkolla om return Page(); behövs eller inte.
            return Page();
        }
    }
}