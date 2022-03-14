using EventiaWebapp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventiaWebapp.Data;

public class EventDbCtx : DbContext
{
    public EventDbCtx(DbContextOptions<EventDbCtx> options)
        : base(options)
    {
    }

    public DbSet<Event> Events { get; set; }
    public DbSet<Organizer> Organizers { get; set; }
    public DbSet<Attendee> Attendees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>().ToTable("Event");
        modelBuilder.Entity<Organizer>().ToTable("Organizer");
        modelBuilder.Entity<Attendee>().ToTable("Attendee");
    }
}