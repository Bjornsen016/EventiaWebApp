using EventiaWebapp.Data;
using EventiaWebapp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventiaWebapp.Services;

public class EventHandler
{
    private EventDbCtx _context;

    public async Task<Event> GetEvent(int? id)
    {
        return await _context.Events.FirstOrDefaultAsync(evt => evt.Id == id);
    }

    public EventHandler(EventDbCtx context)
    {
        _context = context;
    }

    public async Task<List<Event>> GetEventsAsync()
    {
        return await _context.Events.Include(evt => evt.Organizer).Include(evt => evt.Attendees)
            .OrderBy(evt => evt.Date).ToListAsync();
    }

    public async Task<Attendee?> GetAttendeeAsync(int id)
    {
        var attendee = await _context.Attendees.Include(attendee => attendee.Events)
            .ThenInclude(evt => evt.Organizer).FirstOrDefaultAsync(attendee => attendee.Id == id);

        return attendee;
    }

    public async Task<bool> RegisterToEvent(Attendee attendee, Event evt)
    {
        var thisEvent = await _context.Events.FirstOrDefaultAsync(e => e.Id == evt.Id);

        var thisAttendee =
            await _context.Attendees.Include(a => a.Events).FirstOrDefaultAsync(a => a.Id == attendee.Id);

        if (thisAttendee == null || thisEvent == null) return false;

        thisAttendee.Events.Add(thisEvent);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<Event>> GetAttendeeEvents(Attendee attendee)
    {
        //TODO: ta fram endast Events. Behöver inte Attendeen. ELLER?

        var thisAttendee =
            await _context.Attendees.Include(a => a.Events).ThenInclude(evt => evt.Organizer)
                .FirstOrDefaultAsync(a => a.Id == attendee.Id);

        if (thisAttendee == null) throw new Exception("Attendee not found");

        return thisAttendee.Events.ToList();
    }
}