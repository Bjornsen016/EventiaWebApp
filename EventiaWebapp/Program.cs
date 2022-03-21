using EventiaWebapp.Data;
using EventiaWebapp.Services;
using Microsoft.EntityFrameworkCore;
using EventHandler = EventiaWebapp.Services.EventHandler;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<EventDbCtx>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EventDatabase")));
builder.Services.AddScoped<EventHandler>();
builder.Services.AddScoped<LoginHandler>();
builder.Services.AddScoped<Database>();

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

    database.CreateAndSeedIfNotExist();
}
// när vi är i "release" läge
else
{
    app.UseExceptionHandler("/Error");
    database.CreateIfNotExist();
}

scope.Dispose();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();