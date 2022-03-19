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


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();