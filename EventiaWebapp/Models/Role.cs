namespace EventiaWebapp.Models;

public class Role
{
    public int Id { get; set; }
    public string RoleName { get; set; }
    public ICollection<Attendee> Attendees { get; set; }
}