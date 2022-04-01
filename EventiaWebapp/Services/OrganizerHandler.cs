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
        return await _context.Events.Include(e => e.Attendees).Where(e => e.Organizer.Id == organizerId).ToListAsync();
    }
}