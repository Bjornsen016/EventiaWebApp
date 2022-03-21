using EventiaWebapp.Models;
using EventiaWebapp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventiaWebapp.Pages.User
{
    public class IndexModel : PageModel
    {
        private readonly LoginHandler _loginHandler;
        public Attendee Attendee;

        public IndexModel(LoginHandler loginHandler)
        {
            _loginHandler = loginHandler;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var attendeeId = Request.Cookies["attendee"];
            if (attendeeId == null) return RedirectToPage("/LogIn");

            int id = int.Parse(attendeeId);
            Attendee = await _loginHandler.GetAttendee(id);
            return Page();
        }
    }
}