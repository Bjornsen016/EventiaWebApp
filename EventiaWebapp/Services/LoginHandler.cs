using System.Diagnostics.Eventing.Reader;
using System.Security.Claims;
using EventiaWebapp.Data;
using EventiaWebapp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace EventiaWebapp.Services;

/// <summary>
/// Handles logins in the Eventia Web App
/// </summary>
public class LoginHandler
{
    private readonly EventDbCtx _context;

    public LoginHandler(EventDbCtx context)
    {
        _context = context;
    }

    /// <summary>
    /// Tries to log in via the database. Using email and password.
    /// </summary>
    /// <param name="email">Users email</param>
    /// <param name="password">Users password</param>
    /// <returns>A Task that will return the Attendee if the credentials are matching. It will include the Roles.</returns>
    /// <exception cref="LoginException"></exception>
    public async Task<Attendee> TryLoginAttendee(string email, string password)
    {
        var attendee = await _context.Attendees.Include(a => a.Roles)
            .FirstOrDefaultAsync(a => a.Password == password && a.Email == email);
        if (attendee == null) throw new LoginException();
        return attendee;
    }

    /// <summary>
    /// Get an Attendee.
    /// </summary>
    /// <param name="id">The id of the Attendee</param>
    /// <returns>A Task that will return an Attende including its Roles</returns>
    public async Task<Attendee> GetAttendee(int id)
    {
        var attendee = await _context.Attendees.Include(a => a.Roles).FirstOrDefaultAsync(a => a.Id == id);

        return attendee;
    }

    /// <summary>
    /// Creates cookies for the User. 
    /// </summary>
    /// <param name="attendee"></param>
    /// <returns>A Claims Principal with the User cookies</returns>
    public ClaimsPrincipal CreateUserCookies(Attendee attendee)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, attendee.Id.ToString()),
            new Claim(ClaimTypes.Name, attendee.Email)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        return principal;
    }
}

public class LoginException : Exception
{
}