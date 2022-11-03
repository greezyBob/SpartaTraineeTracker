using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using TraineeTrackerApp.Models;

namespace TraineeTrackerApi.Data.DTO;

public class WeekDTO
{
    public int Id { get; set; }
    public string Start { get; set; }
    public string Stop { get; set; }
    public string Continue { get; set; }
    public string WeekStart { get; set; }
    public string GitHubLink { get; set; }
    public string TechnicalSkill { get; set; }
    public string ConsultantSkill { get; set; }
}
