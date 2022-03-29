using EventiaWebapp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EventiaWebapp.Data;

public class EventDbCtx : IdentityDbContext<User, IdentityRole, string>
{
    public EventDbCtx(DbContextOptions<EventDbCtx> options)
        : base(options)
    {
    }

    public DbSet<Event> Events { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseSqlServer(@$"Server=(localdb)\MSSQLLocalDB;Database=EventDb");
        }
    }
}