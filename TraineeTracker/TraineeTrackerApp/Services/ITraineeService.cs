using TraineeTrackerApp.Models;

namespace TraineeTrackerApp.Services
{
    public interface ITraineeService
    {
        public Task<List<Spartan>> GetSpartansAsync();
        public Task<Spartan?> GetSpartanByIdAsync(string? id);
        public Task<List<Week>> GetWeeksBySpartanIdAsync(string? id);
    }
}
