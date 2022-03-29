using EventiaWebapp.Data;
using EventiaWebapp.Models;

namespace EventiaWebapp.Services;

/// <summary>
/// Handles the database for Eventia Web App.
/// Admin control.
/// </summary>
public class Database
{
    private readonly EventDbCtx _context;

    public Database(EventDbCtx context)
    {
        _context = context;
    }

    /// <summary>
    /// Deletes, recreates and Seeds the database with test data
    /// </summary>
    public void RecreateAndSeed()
    {
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        Seed();
    }

    /// <summary>
    /// If the database does not exist it will be created. Otherwise nothing will happen.
    /// </summary>
    public void CreateIfNotExist()
    {
        _context.Database.EnsureCreated();
    }

    /// <summary>
    /// If the database does not exist it will be created and seeded with test data. Otherwise nothing will happen.
    /// </summary>
    public void CreateAndSeedIfNotExist()
    {
        if (!_context.Database.EnsureCreated()) return;
        Seed();
    }

    /// <summary>
    /// Seeds the database with test data.
    /// </summary>
    private void Seed()
    {
        var roles = new Role[]
        {
            new() {RoleName = "admin"},
            new() {RoleName = "user"}
        };
        var org = new Organizer() {Email = "Kim@mail.com", Name = "Kims Dataspel", PhoneNumber = "07022222"};
        var orgTwo = new Organizer() {Email = "Two@mail.com", Name = "Kims andra events", PhoneNumber = "01828018"};

        var events = new Event[]
        {
            new()
            {
                Title = "Lan Party ",
                Description =
                    "Curabitur maximus commodo mauris id venenatis. Sed viverra cursus sagittis. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nullam finibus pulvinar augue a dignissim. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque ut imperdiet lorem, quis egestas sem. Proin tincidunt tortor eleifend massa tempor gravida. Sed eu luctus est.",
                Address = "Aggetorp", Date = new DateTime(2022, 05, 10, 19, 0, 0).ToUniversalTime(), UtcTimeOffset = 1,
                Place = "Gråbo", SpotsAvailable = 10, Organizer = org
            },
            new()
            {
                Title = "Spela",
                Description =
                    "Curabitur maximus commodo mauris id venenatis. Sed viverra cursus sagittis. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nullam finibus pulvinar augue a dignissim. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque ut imperdiet lorem, quis egestas sem. Proin tincidunt tortor eleifend massa tempor gravida. Sed eu luctus est.",
                Address = "Aggetorp",
                Date = new DateTime(2022, 05, 10, 19, 0, 0).ToUniversalTime(),
                UtcTimeOffset = 1,
                Place = "Gråbo", SpotsAvailable = 10, Organizer = org
            },
            new()
            {
                Title = "Spela",
                Description =
                    "Curabitur maximus commodo mauris id venenatis. Sed viverra cursus sagittis. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nullam finibus pulvinar augue a dignissim. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque ut imperdiet lorem, quis egestas sem. Proin tincidunt tortor eleifend massa tempor gravida. Sed eu luctus est.",
                Address = "Aggetorp",
                Date = new DateTime(2022, 05, 10, 19, 0, 0).ToUniversalTime(),
                UtcTimeOffset = 1,
                Place = "Gråbo", SpotsAvailable = 10, Organizer = org
            },
            new()
            {
                Title = "Lan Party ",
                Description =
                    "Curabitur maximus commodo mauris id venenatis. Sed viverra cursus sagittis. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nullam finibus pulvinar augue a dignissim. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque ut imperdiet lorem, quis egestas sem. Proin tincidunt tortor eleifend massa tempor gravida. Sed eu luctus est.",
                Address = "Aggetorp", Date = new DateTime(2022, 05, 10, 19, 0, 0).ToUniversalTime(), UtcTimeOffset = 1,
                Place = "Gråbo", SpotsAvailable = 1, Organizer = org
            },
            new()
            {
                Title = "Något kul ",
                Description =
                    "Curabitur maximus commodo mauris id venenatis. Sed viverra cursus sagittis. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nullam finibus pulvinar augue a dignissim. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque ut imperdiet lorem, quis egestas sem. Proin tincidunt tortor eleifend massa tempor gravida. Sed eu luctus est.",
                Address = "Aggetorp", Date = new DateTime(2022, 09, 10, 19, 0, 0).ToUniversalTime(), UtcTimeOffset = 1,
                Place = "Gråbo", SpotsAvailable = 10, Organizer = orgTwo
            },
            new()
            {
                Title = "Fia med knuff",
                Description =
                    "Curabitur maximus commodo mauris id venenatis. Sed viverra cursus sagittis. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nullam finibus pulvinar augue a dignissim. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque ut imperdiet lorem, quis egestas sem. Proin tincidunt tortor eleifend massa tempor gravida. Sed eu luctus est.",
                Address = "Aggetorp", Date = new DateTime(2022, 07, 10, 19, 0, 0).ToUniversalTime(), UtcTimeOffset = 1,
                Place = "Gråbo", SpotsAvailable = 10, Organizer = orgTwo
            },
            new()
            {
                Title = "Sticknings kväll",
                Description =
                    "Curabitur maximus commodo mauris id venenatis. Sed viverra cursus sagittis. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nullam finibus pulvinar augue a dignissim. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque ut imperdiet lorem, quis egestas sem. Proin tincidunt tortor eleifend massa tempor gravida. Sed eu luctus est.",
                Address = "Aggetorp", Date = new DateTime(2022, 08, 10, 19, 0, 0).ToUniversalTime(), UtcTimeOffset = 1,
                Place = "Gråbo", SpotsAvailable = 10, Organizer = orgTwo
            },
        };

        var kimsEvents = new[]
        {
            events[0],
            events[1]
        };

        var markusEvents = new[]
        {
            events[3], events[5]
        };

        var attendees = new User[]
        {
            new()
            {
                Email = "kims@email.com", Password = "password", Name = "Kim", PhoneNumber = "0021021013",
                Events = kimsEvents, Roles = new List<Role> {roles[0], roles[1]}
            },
            new()
            {
                Email = "Markus@email.com", Password = "password", Name = "Markus", PhoneNumber = "0021021013",
                Events = markusEvents, Roles = new List<Role> {roles[1]}
            }
        };

        _context.Events.AddRange(events);
        _context.Attendees.AddRange(attendees);
        _context.SaveChanges();
    }
}