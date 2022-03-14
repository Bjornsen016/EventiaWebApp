using System.ComponentModel.DataAnnotations;

namespace EventiaWebapp.Models;

public class Organizer
{
    public int Id { get; set; }
    [Required] public string Name { get; set; }
    [Required] public string Email { get; set; }
    [Required] public string PhoneNumber { get; set; }
    public ICollection<Event> Events { get; set; }
}