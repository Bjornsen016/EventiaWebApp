using EventiaWebapp.Data;
using EventiaWebapp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventiaWebapp.Services;

/// <summary>
/// Handles events for the Eventia Web App.
/// </summary>
public class EventHandler
{
    private readonly EventDbCtx _context;

    public EventHandler(EventDbCtx context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets the specific Event from the database.
    /// </summary>
    /// <param name="id">id of the event</param>
    /// <returns>A Task that will return the Event including its Attendees if it's found, otherwise null</returns>
    public async Task<Event> GetEvent(int? id)
    {
        return await _context.Events.Include(evt => evt.Attendees).FirstOrDefaultAsync(evt => evt.Id == id);
    }

    /// <summary>
    /// Creates a new Event
    /// </summary>
    /// <param name="address"></param>
    /// <param name="place"></param>
    /// <param name="organizer"></param>
    /// <param name="date"></param>
    /// <param name="utcTimeOffset"></param>
    /// <param name="title"></param>
    /// <param name="description"></param>
    /// <param name="spotsAvailable"></param>
    /// <returns>A Task that returns true if the Event has been pushed to the database.</returns>
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

    /// <summary>
    /// Gets all the Events in the database.
    /// </summary>
    /// <returns>A Task that returns a List of all the Events in the database. Includes the Attendees that have signed up</returns>
    public async Task<List<Event>> GetEventsAsync()
    {
        return await _context.Events.Include(evt => evt.Organizer).Include(evt => evt.Attendees)
            .OrderBy(evt => evt.Date).ToListAsync();
    }

    /// <summary>
    /// Gets a specific User including the Events that they are signed up to.
    /// </summary>
    /// <param name="id">id of the User</param>
    /// <returns>A Task that will return the User, its Events and the Organizer of those events.</returns>
    public async Task<User?> GetAttendeeAsync(int id)
    {
        var attendee = await _context.Attendees.Include(attendee => attendee.Events)
            .ThenInclude(evt => evt.Organizer).FirstOrDefaultAsync(attendee => attendee.Id == id);

        return attendee;
    }

    /// <summary>
    /// Register an User to a specific Event
    /// </summary>
    /// <param name="userndee">The User that wants to join</param>
    /// <param name="evt">The Event to register to</param>
    /// <returns>A Task that will return true if the User is successfully registered to the Event</returns>
    /// <exception cref="SpotsFilledException"></exception>
    public async Task<bool> RegisterToEvent(User user, Event evt)
    {
        var thisEvent = await _context.Events.Include(e => e.Attendees).FirstOrDefaultAsync(e => e.Id == evt.Id);

        var thisAttendee = await GetAttendeeAsync(user.Id);

        //TODO: Throw exception if event is full, so we can handle it in the frontend later.
        if (thisAttendee == null || thisEvent == null) return false;
        if (thisEvent.Attendees.Count >= thisEvent.SpotsAvailable) throw new SpotsFilledException();

        thisAttendee.Events.Add(thisEvent);
        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Get the specific Events the User is signed up for.
    /// </summary>
    /// <param name="userndee"></param>
    /// <returns>A Task that will return a List of Events that the User is signed up for</returns>
    /// <exception cref="Exception"></exception>
    public async Task<List<Event>> GetAttendeeEvents(User user)
    {
        var thisAttendee =
            await _context.Attendees.Include(a => a.Events).ThenInclude(evt => evt.Organizer)
                .FirstOrDefaultAsync(a => a.Id == user.Id);

        if (thisAttendee == null) throw new Exception("User not found");

        return thisAttendee.Events.OrderBy(evt => evt.Date).ToList();
    }
}

public class SpotsFilledException : Exception
{
}