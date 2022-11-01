using System.ComponentModel.DataAnnotations;

namespace TraineeTrackerApp.Models;

public class Course
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; } = null!;
}