using EventiaWebapp.Data;
using EventiaWebapp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventiaWebapp.Services;

public class OrganizerHandler
{
    private readonly EventDbCtx _context;

    public OrganizerHandler(EventDbCtx context)
    {
        _context = context;
    }

    public async Task<List<Event>> GetOrganizersEvents(string organizerId)
    {
        return await _context.Events.Include(e => e.Attendees).Where(e => e.Organizer.Id == organizerId)
            .OrderBy(e => e.Date).ToListAsync();
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
    public async Task<bool> CreateNewEvent(string address, string place, User organizer, DateTime date,
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

    public async Task<bool> EditEvent(Event currentEvent, string inputAdress, string inputPlace, DateTime inputDate,
        int inputUtcOffset, string inputTitle, string inputDescription, int inputSpotsAvailable)
    {
        var dbEvent = await _context.Events.FirstOrDefaultAsync(e => e.Id == currentEvent.Id);

        if (dbEvent is null) return false;

        dbEvent.Address = inputAdress;
        dbEvent.UtcTimeOffset = inputUtcOffset;
        dbEvent.Title = inputTitle;
        dbEvent.Description = inputDescription;
        dbEvent.SpotsAvailable = inputSpotsAvailable;
        dbEvent.Place = inputPlace;
        dbEvent.Date = inputDate;

        await _context.SaveChangesAsync();
        return true;
    }
}