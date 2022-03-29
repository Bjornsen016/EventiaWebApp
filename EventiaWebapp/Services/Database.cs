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
            new() {Name = "administrator"},
            new() {Name = "attendee"},
            new() {Name = "organizer"}
        };

        await _roleManager.CreateAsync(roles[0]);
        await _roleManager.CreateAsync(roles[1]);
        await _roleManager.CreateAsync(roles[2]);

        var admin = new User
        {
            Email = "admin@eventia.com", UserName = "admin@eventia.com", FirstName = "admin", LastName = "",
            PhoneNumber = "21312314"
        };
        await _userManager.CreateAsync(admin, "4Dministrator!");
        await _userManager.AddToRoleAsync(admin, "administrator");
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
                Email = "kims@email.com", FirstName = "Kim", LastName = "Björnsen Åklint", PhoneNumber = "0021021013"
            },
            new()
            {
                Email = "Markus@email.com", FirstName = "Markus", LastName = "Guru", PhoneNumber = "0021021013"
            }
        };
        await _userManager.CreateAsync(users[0], "P4ssword!");
        await _userManager.CreateAsync(users[1], "P4ssword!");

        await _userManager.AddToRolesAsync(users[0], new List<string> {"attendee", "organizer"});
        await _userManager.AddToRoleAsync(users[1], "attendee");
    }
}