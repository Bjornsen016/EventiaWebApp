using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace EventiaWebapp.Models;

public class User : IdentityUser
{
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }

    public List<Event>? HostedEvents { get; set; }

    public List<Event>? JoinedEvents { get; set; }
}