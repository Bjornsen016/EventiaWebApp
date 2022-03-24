using System.Diagnostics.Eventing.Reader;
using System.Security.Claims;
using EventiaWebapp.Data;
using EventiaWebapp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace EventiaWebapp.Services;

public class LoginHandler
{
    private readonly EventDbCtx _context;

    public LoginHandler(EventDbCtx context)
    {
        _context = context;
    }

    public async Task<Attendee> TryLoginAttendee(string email, string password)
    {
        var attendee = await _context.Attendees.Include(a => a.Roles)
            .FirstOrDefaultAsync(a => a.Password == password && a.Email == email);
        if (attendee == null) throw new LoginException();
        return attendee;
    }

    public async Task<Attendee> GetAttendee(int id)
    {
        var attendee = await _context.Attendees.Include(a => a.Roles).FirstOrDefaultAsync(a => a.Id == id);

        return attendee;
    }

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