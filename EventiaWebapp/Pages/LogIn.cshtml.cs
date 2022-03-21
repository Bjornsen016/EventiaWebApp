using System.ComponentModel.DataAnnotations;
using EventiaWebapp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventiaWebapp.Pages
{
    public class LogInModel : PageModel
    {
        private readonly LoginHandler _loginHandler;
        public string LoginFailedMessage { get; set; }

        [EmailAddress] [BindProperty] public string Email { get; set; }

        [BindProperty] public string Password { get; set; }

        public LogInModel(LoginHandler loginHandler)
        {
            _loginHandler = loginHandler;
        }

        public IActionResult OnGet()
        {
            if (Request.Cookies["attendee"] != null)
            {
                return RedirectToPage("/MyEvents");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            try
            {
                var user = await _loginHandler.TryLoginAttendee(Email, Password);

                var options = new CookieOptions
                {
                    Expires = DateTime.Today.AddDays(2)
                };

                Response.Cookies.Append("attendee", user.Id.ToString(), options);
                return RedirectToPage("/MyEvents");
            }
            catch (LoginException ex)
            {
                LoginFailedMessage = "Password is wrong.";
            }

            return Page();
        }
    }
}