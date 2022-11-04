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

    public static SpartanDTO SpartanToDTO(Spartan spartan)
    {
        return new SpartanDTO
        {
            Email = spartan.Email,
            Roles = new List<string>(),
            Firstname = spartan.FirstName,
            Lastname = spartan.LastName,
            StartDate = spartan.StartDate.ToString("yyyy-MM-dd") ?? "",
            WeeksCount = spartan.Weeks.Count
        };
    }
}
