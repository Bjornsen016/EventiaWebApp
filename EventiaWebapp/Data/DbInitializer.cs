using EventiaWebapp.Models;

namespace EventiaWebapp.Data;

public static class DbInitializer
{
    public static void Initialize(EventDbCtx context)
    {
        if (context.Events.Any()) return;

        var org = new Organizer() {Email = "Kim@mail", Name = "Kims Dataspel", PhoneNumber = "07022222"};
        var orgTwo = new Organizer() {Email = "Two@mail.com", Name = "Kims andra events", PhoneNumber = "01828018"};

        var events = new Event[]
        {
            new()
            {
                Title = "Lan Party ",
                Description =
                    "Curabitur maximus commodo mauris id venenatis. Sed viverra cursus sagittis. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nullam finibus pulvinar augue a dignissim. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque ut imperdiet lorem, quis egestas sem. Proin tincidunt tortor eleifend massa tempor gravida. Sed eu luctus est.",
                Address = "Aggetorp", Date = new DateTime(2022, 05, 10),
                Place = "Gråbo", SpotsAvailable = 10, Organizer = org
            },
            new()
            {
                Title = "Spela", Description = "Datorspel", Address = "Aggetorp", Date = new DateTime(2022, 05, 10),
                Place = "Gråbo", SpotsAvailable = 10, Organizer = org
            },
            new()
            {
                Title = "Spela", Description = "Datorspel", Address = "Aggetorp", Date = new DateTime(2022, 05, 10),
                Place = "Gråbo", SpotsAvailable = 10, Organizer = org
            },
            new()
            {
                Title = "Lan Party ",
                Description =
                    "Curabitur maximus commodo mauris id venenatis. Sed viverra cursus sagittis. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nullam finibus pulvinar augue a dignissim. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque ut imperdiet lorem, quis egestas sem. Proin tincidunt tortor eleifend massa tempor gravida. Sed eu luctus est.",
                Address = "Aggetorp", Date = new DateTime(2022, 05, 10),
                Place = "Gråbo", SpotsAvailable = 10, Organizer = org
            },
            new()
            {
                Title = "Något kul ",
                Description =
                    "Curabitur maximus commodo mauris id venenatis. Sed viverra cursus sagittis. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nullam finibus pulvinar augue a dignissim. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque ut imperdiet lorem, quis egestas sem. Proin tincidunt tortor eleifend massa tempor gravida. Sed eu luctus est.",
                Address = "Aggetorp", Date = new DateTime(2022, 09, 10),
                Place = "Gråbo", SpotsAvailable = 10, Organizer = orgTwo
            },
            new()
            {
                Title = "Fia med knuff",
                Description =
                    "Curabitur maximus commodo mauris id venenatis. Sed viverra cursus sagittis. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nullam finibus pulvinar augue a dignissim. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque ut imperdiet lorem, quis egestas sem. Proin tincidunt tortor eleifend massa tempor gravida. Sed eu luctus est.",
                Address = "Aggetorp", Date = new DateTime(2022, 07, 10),
                Place = "Gråbo", SpotsAvailable = 10, Organizer = orgTwo
            },
            new()
            {
                Title = "Sticknings kväll",
                Description =
                    "Curabitur maximus commodo mauris id venenatis. Sed viverra cursus sagittis. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nullam finibus pulvinar augue a dignissim. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque ut imperdiet lorem, quis egestas sem. Proin tincidunt tortor eleifend massa tempor gravida. Sed eu luctus est.",
                Address = "Aggetorp", Date = new DateTime(2022, 08, 10),
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
            events[4], events[5]
        };

        var attendees = new Attendee[]
        {
            new()
            {
                Email = "kims@email.com", Password = "password", Name = "Kim", PhoneNumber = "0021021013",
                Events = kimsEvents
            },
            new()
            {
                Email = "Markus@email.com", Password = "password", Name = "Markus", PhoneNumber = "0021021013",
                Events = markusEvents
            }
        };

        context.Events.AddRange(events);
        context.Attendees.AddRange(attendees);
        context.SaveChanges();
    }
}