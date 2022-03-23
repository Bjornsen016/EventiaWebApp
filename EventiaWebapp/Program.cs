using EventiaWebapp.Data;
using EventiaWebapp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using EventHandler = EventiaWebapp.Services.EventHandler;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<EventDbCtx>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EventDatabase")));
builder.Services.AddScoped<EventHandler>();
builder.Services.AddScoped<LoginHandler>();
builder.Services.AddScoped<Database>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/LogIn";
        options.LogoutPath = "/LogOut";
        options.ReturnUrlParameter = "ReturnUrl";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
    });

// n�r vi �r i "debug" l�ge
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}


var app = builder.Build();
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var database = services.GetRequiredService<Database>();
// n�r vi �r i "debug" l�ge
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    database.CreateAndSeedIfNotExist();
}
// n�r vi �r i "release" l�ge
else
{
    app.UseExceptionHandler("/Error");
    database.CreateIfNotExist();
}

scope.Dispose();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();