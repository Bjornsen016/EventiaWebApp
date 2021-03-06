using EventiaWebapp.Data;
using EventiaWebapp.Models;
using Microsoft.AspNetCore.Identity;

namespace EventiaWebapp.Services;

/// <summary>
/// Handles the database for Eventia Web App.
/// Admin control.
/// </summary>
public class Database
{
    private readonly EventDbCtx _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public Database(EventDbCtx context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    /// <summary>
    /// Deletes, recreates and Seeds the database with test data
    /// </summary>
    public async Task RecreateAndSeed()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();
        await Seed();
    }

    /// <summary>
    /// If the database does not exist it will be created. Otherwise nothing will happen.
    /// </summary>
    public async Task CreateIfNotExist()
    {
        if (!await _context.Database.EnsureCreatedAsync()) return;
        await SeedRolesAndAdmin();
    }

    /// <summary>
    /// If the database does not exist it will be created and seeded with test data. Otherwise nothing will happen.
    /// </summary>
    public async Task CreateAndSeedIfNotExist()
    {
        if (!await _context.Database.EnsureCreatedAsync()) return;
        await Seed();
    }


    private async Task SeedRolesAndAdmin()
    {
        var roles = new IdentityRole[]
        {
            new() {Name = Config.ADMIN_ROLE_NAME},
            new() {Name = Config.ATTENDEE_ROLE_NAME},
            new() {Name = Config.ORGANIZER_ROLE_NAME}
        };

        await _roleManager.CreateAsync(roles[0]);
        await _roleManager.CreateAsync(roles[1]);
        await _roleManager.CreateAsync(roles[2]);

        var admin = new User
        {
            Email = "admin@eventia.com", UserName = "admin@eventia.com", FirstName = "admin", LastName = "",
            PhoneNumber = "21312314"
        };
        await _userManager.CreateAsync(admin, "AdminP4ssword!");
        await _userManager.AddToRoleAsync(admin, Config.ADMIN_ROLE_NAME);
    }

    /// <summary>
    /// Seeds the database with test data.
    /// </summary>
    private async Task Seed()
    {
        await SeedRolesAndAdmin();

        var users = new User[]
        {
            new()
            {
                Email = "kims@email.com", UserName = "Events_a_la_Kim", FirstName = "Kim", LastName = "Björnsen Åklint",
                PhoneNumber = "0021021013"
            },
            new()
            {
                Email = "Markus@email.com", UserName = "Markus@email.com", FirstName = "Markus", LastName = "Guru",
                PhoneNumber = "0021021013"
            }
        };

        await _userManager.CreateAsync(users[0], "P4ssword!");
        await _userManager.CreateAsync(users[1], "P4ssword!");

        await _userManager.AddToRolesAsync(users[0],
            new List<string> {Config.ATTENDEE_ROLE_NAME, Config.ORGANIZER_ROLE_NAME});
        await _userManager.AddToRoleAsync(users[1], Config.ATTENDEE_ROLE_NAME);

        var events = new Event[]
        {
            new()
            {
                Address = "Gatan i Varberg", Date = new DateTime(2022, 12, 31),
                Description = "New years party, bring your friends and enjoy the fireworks", Title = "New Years Party",
                Place = "Party place, Varberg", UtcTimeOffset = 1, SpotsAvailable = 100, Organizer = users[0],
                Attendees = new List<User> {users[0], users[1]}
            },
            new()
            {
                Address = "Avenyn 19", Date = new DateTime(2022, 06, 02), Description = "Coolest party",
                Title = "Cool party", Place = "Party place, Göteborg", UtcTimeOffset = 1, SpotsAvailable = 10,
                Organizer = users[0], Attendees = new List<User> {users[1]}
            },
            new()
            {
                Address = "Avenyn 19", Date = new DateTime(2022, 07, 15),
                Description = "This party is the best you'll ever join", Title = "Best Party",
                Place = "Party place, Göteborg", UtcTimeOffset = 1, SpotsAvailable = 15, Organizer = users[0]
            },
            new()
            {
                Address = "Mitt i ingenstans 14", Date = new DateTime(2022, 05, 20),
                Description = "Bring your own drinks, the rest is on us", Title = "Pool Party", Place = "Poolhuset",
                UtcTimeOffset = 1, SpotsAvailable = 5, Organizer = users[0], Attendees = new List<User> {users[1]}
            }
        };

        await _context.Events.AddRangeAsync(events);
        await _context.SaveChangesAsync();
    }
}