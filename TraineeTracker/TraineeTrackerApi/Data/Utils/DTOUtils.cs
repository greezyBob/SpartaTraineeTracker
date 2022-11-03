using TraineeTrackerApi.Data.DTO;
using TraineeTrackerApp.Models;

namespace TraineeTrackerApi.Data.Utils;

public static class DTOUtils
{
    public static WeekDTO WeekToDto(Week week)
    {
        return new WeekDTO
        {
            Id = week.Id,
            Start = week.Start ?? "",
            Stop = week.Stop ?? "",
            Continue = week.Continue ?? "",
            WeekStart = week.WeekStart.ToString("yyyy-MM-dd") ?? "",
            GitHubLink = week.GitHubLink ?? "",
            TechnicalSkill = ((Proficiency)week.TechnicalSkill).ToString() ?? "",
            ConsultantSkill = ((Proficiency)week.ConsultantSkill).ToString() ?? ""
        };
    }
}
