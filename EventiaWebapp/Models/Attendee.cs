using System.ComponentModel.DataAnnotations;

namespace EventiaWebapp.Models;

public class Attendee
{
    public int Id { get; set; }
    [Required] public string Name { get; set; }
    [Required] public string Email { get; set; }
    [Required] public string Password { get; set; }
    [Required] public string PhoneNumber { get; set; }
    public ICollection<Event> Events { get; set; }
    public ICollection<Role> Roles { get; set; }

    public bool IsAdmin()
    {
        return Roles.FirstOrDefault(r => r.RoleName == "admin") != null;
    }
}