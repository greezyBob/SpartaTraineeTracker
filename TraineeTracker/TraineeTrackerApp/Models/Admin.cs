using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TraineeTrackerApp.Models;

public class Admin : IdentityUser
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
}