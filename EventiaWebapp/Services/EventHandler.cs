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
    public async Task<User?> GetUserAsync(string id)
    {
        var user = await _context.Users.Include(user => user.JoinedEvents)
            .ThenInclude(evt => evt.Organizer).FirstOrDefaultAsync(user => user.Id == id);

        return user;
    }

    /// <summary>
    /// Register an User to a specific Event
    /// </summary>
    /// <param name="user">The User that wants to join</param>
    /// <param name="evt">The Event to register to</param>
    /// <returns>A Task that will return true if the User is successfully registered to the Event</returns>
    /// <exception cref="SpotsFilledException"></exception>
    public async Task<bool> RegisterToEvent(User user, Event evt)
    {
        var thisEvent = await _context.Events.Include(e => e.Attendees).FirstOrDefaultAsync(e => e.Id == evt.Id);

        var thisUser = await GetUserAsync(user.Id);

        if (thisUser == null || thisEvent == null) return false;
        if (thisEvent.Attendees.Count >= thisEvent.SpotsAvailable) throw new SpotsFilledException();

        thisUser.JoinedEvents.Add(thisEvent);
        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Uregister the user from the event.
    /// </summary>
    /// <param name="user">The user that wants to unregister</param>
    /// <param name="evt">The event that the user wants to unregister from</param>
    /// <returns>True if succeded. Else False.</returns>
    public async Task<bool> UnRegisterToEvent(User user, Event evt)
    {
        var thisEvent = await _context.Events.Include(e => e.Attendees).FirstOrDefaultAsync(e => e.Id == evt.Id);

        var thisUser = await GetUserAsync(user.Id);

        if (thisUser == null || thisEvent == null) return false;

        thisUser.JoinedEvents.Remove(thisEvent);
        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Get the specific Events the User is signed up for.
    /// </summary>
    /// <param name="user"></param>
    /// <returns>A Task that will return a List of Events that the User is signed up for</returns>
    /// <exception cref="Exception"></exception>
    public async Task<List<Event>> GetAttendeeEvents(User user)
    {
        var thisUser =
            await _context.Users.Include(a => a.JoinedEvents).ThenInclude(evt => evt.Organizer)
                .FirstOrDefaultAsync(a => a.Id == user.Id);

        if (thisUser == null) throw new Exception("User not found");

        return thisUser.JoinedEvents.OrderBy(evt => evt.Date).ToList();
    }
}

public class SpotsFilledException : Exception
{
}