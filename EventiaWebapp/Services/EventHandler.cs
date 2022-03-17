using EventiaWebapp.Data;
using EventiaWebapp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventiaWebapp.Services;

public class EventHandler
{
    private EventDbCtx _context;

    public EventHandler(EventDbCtx context)
    {
        _context = context;
    }

    public async Task<Event> GetEvent(int? id)
    {
        return await _context.Events.FirstOrDefaultAsync(evt => evt.Id == id);
    }

    public async Task<bool> CreateNewEvent(string address, string place, Organizer organizer, DateTime date,
        int utcTimeOffset, string title, string description, int spotsAvailable)
    {
        var newEvent = new Event
        {
            Address = address,
            Place = place,
            Organizer = organizer,
            Date = date.ToUniversalTime(),
            UtcTimeOffset = utcTimeOffset,
            Title = title,
            Description = description,
            SpotsAvailable = spotsAvailable
        };

        await _context.Events.AddAsync(newEvent);
        int numberOfPushedEvents = await _context.SaveChangesAsync();

        return numberOfPushedEvents == 1;
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
        var thisEvent = await _context.Events.Include(e => e.Attendees).FirstOrDefaultAsync(e => e.Id == evt.Id);

        var thisAttendee =
            await _context.Attendees.Include(a => a.Events).FirstOrDefaultAsync(a => a.Id == attendee.Id);

        //TODO: Throw exception if event is full, so we can handle it in the frontend later.
        if (thisAttendee == null || thisEvent == null) return false;
        if (thisEvent.Attendees.Count >= thisEvent.SpotsAvailable) throw new SpotsFilledException();

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

        return thisAttendee.Events.OrderBy(evt => evt.Date).ToList();
    }
}

public class SpotsFilledException : Exception
{
}