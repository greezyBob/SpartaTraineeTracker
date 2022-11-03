using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TraineeTrackerApp.Models;

public class Spartan : IdentityUser
{
    public Spartan() { 
        Weeks = new HashSet<Week>();
    }

    [Required]
    [DisplayName("First Name")]
    public string FirstName { get; set; }

    [Required]
    [DisplayName("Last Name")]
    public string LastName { get; set; }

    [DisplayName("Address 1")]
    public string? Address1 { get; set; }

    [DisplayName("Address 2")]
    public string? Address2 { get; set; }

    [DisplayName("Post Code")]
    public string? PostCode { get; set; }

    [DisplayName("Start Date")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; } = DateTime.Now;

    public virtual ICollection<Course> Course { get; set; }

    public virtual ICollection<Week> Weeks { get; set; }
}