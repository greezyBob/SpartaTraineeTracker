using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraineeTrackerApp.Models;

public class Week
{
    public int Id { get; set; }
    [Required]
    public string Start { get; set; }
    [Required]
    public string Stop { get; set; }
    [Required]
    public string Continue { get; set; }
    [Required]
    [DataType(DataType.Date)]
    public DateTime WeekStart { get; set; }
    public string? GitHubLink { get; set; }
    [Required]
    [Display(Name = "Technical Skill")]
    public Proficiency TechnicalSkill { get; set; }
    [Required]
    [Display(Name = "Consultant Skill")]
    public Proficiency ConsultantSkill { get; set; }
    [ForeignKey("Spartan")]
    public string? SpartanId { get; set; }
    public Spartan Spartan { get; set; }
}

public enum Proficiency
{
    Skilled,
    PartiallySkilled,
    LowSkilled,
    Unskilled
}