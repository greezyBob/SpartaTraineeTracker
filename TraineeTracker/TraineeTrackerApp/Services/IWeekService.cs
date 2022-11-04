using TraineeTrackerApp.Models;

namespace TraineeTrackerApp.Services
{
    public interface IWeekService
    {
        public Task<List<Week>> GetWeeksAsync();
        public Task<Week?> GetWeekByIdAsync(int? id);
        public Task<List<Spartan>> GetSpartansAsync();
        public Task<Spartan?> GetSpartansByIdAsync(int? id);
        public Task AddWeek(Week week);
        public Task SaveChangesAsync();
        public Task RemoveWeekAsync(Week week);
    }
}
