namespace TraineeTrackerApi.Data.DTO;

public class SpartanDTO
{
    public string Email { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string StartDate { get; set; }
    public int WeeksCount { get; set; }
    public ICollection<string> Roles { get; set; }
}
