using EventiaWebapp.Data;
using EventiaWebapp.Models;
using EventiaWebapp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EventHandler = EventiaWebapp.Services.EventHandler;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<EventDbCtx>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EventDatabase")));
builder.Services.AddScoped<EventHandler>();
builder.Services.AddScoped<Database>();

builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzåäöABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖ0123456789-._@+";
    })
    .AddEntityFrameworkStores<EventDbCtx>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;

    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.LoginPath = "/User/Login";
    options.SlidingExpiration = true;
    options.ReturnUrlParameter = "ReturnUrl";
});

// när vi är i "debug" läge
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}


var app = builder.Build();
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var database = services.GetRequiredService<Database>();

// när vi är i "debug" läge
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    await database.CreateAndSeedIfNotExist();
}
// när vi är i "release" läge
else
{
    app.UseExceptionHandler("/Error");
    await database.CreateIfNotExist();
}

scope.Dispose();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();