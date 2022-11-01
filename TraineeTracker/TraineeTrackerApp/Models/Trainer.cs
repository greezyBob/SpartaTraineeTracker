using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TraineeTrackerApp.Models;

public class Trainer : IdentityUser 
{
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    public virtual ICollection<Course> Courses { get; set; }
    public virtual ICollection<Spartan> Spartans { get; set; }
}