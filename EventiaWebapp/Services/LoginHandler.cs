using System.Diagnostics.Eventing.Reader;
using EventiaWebapp.Data;
using EventiaWebapp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventiaWebapp.Services;

public class LoginHandler
{
    private readonly EventDbCtx _context;

    public LoginHandler(EventDbCtx context)
    {
        _context = context;
    }

    public async Task<Attendee> TryLoginAttendee(string password)
    {
        var attendee = await _context.Attendees.FirstOrDefaultAsync(a => a.Password == password && a.Id == 1);
        if (attendee == null) throw new LoginException();
        return attendee;
    }
}

public class LoginException : Exception
{
}