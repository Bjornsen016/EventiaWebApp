using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventiaWebapp.Pages.User;

[AllowAnonymous]
public class RegisterModel : PageModel
{
    private readonly UserManager<Models.User> _userManager;
    private readonly ILogger<RegisterModel> _logger;

    [Required]
    [EmailAddress]
    [BindProperty]
    [Display(Name = "Email Address")]
    public string Email { get; set; }

    [BindProperty] public string Username { get; set; }

    [BindProperty]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [BindProperty]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [Required]
    [BindProperty]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Phone number")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [BindProperty]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirmation Password is required.")]
    [BindProperty]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
    public string ConfirmPassword { get; set; }

    public RegisterModel(UserManager<Models.User> userManager, ILogger<RegisterModel> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        Models.User newUser = new Models.User
        {
            Email = Email, UserName = Username, FirstName = FirstName = FirstName, LastName = LastName,
            PhoneNumber = PhoneNumber
        };

        var created = await _userManager.CreateAsync(newUser, Password);

        if (created.Succeeded)
        {
            var roledAdded = await _userManager.AddToRoleAsync(newUser, "attendee");
            _logger.LogInformation("New user has been created!");
            return RedirectToPage("/User/Login");
        }

        foreach (var error in created.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }

        return Page();
    }
}