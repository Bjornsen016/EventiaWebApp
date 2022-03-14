using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventiaWebapp.Models;

public class Event
{
    public int Id { get; set; }
    [Required] public string Title { get; set; }
    [Required] public string Description { get; set; }
    [Required] public string Place { get; set; }
    [Required] public string Address { get; set; }
    [Required] [Column(TypeName = "date")] public DateTime Date { get; set; }
    [Required] public int SpotsAvailable { get; set; }
    [Required] public Organizer Organizer { get; set; }
    public ICollection<Attendee> Attendees { get; set; }
}