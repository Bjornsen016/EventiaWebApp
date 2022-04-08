using System.ComponentModel;
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
    [Required] public DateTime Date { get; set; }
    [Required] public int UtcTimeOffset { get; set; }
    [Required] public int SpotsAvailable { get; set; }

    [InverseProperty("HostedEvents")]
    [DisplayName("Organizer")]
    public User Organizer { get; set; }

    [InverseProperty("JoinedEvents")] public List<User>? Attendees { get; set; }
}