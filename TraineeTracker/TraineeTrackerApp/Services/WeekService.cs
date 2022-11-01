using Microsoft.EntityFrameworkCore;
using TraineeTrackerApp.Data;
using TraineeTrackerApp.Models;

namespace TraineeTrackerApp.Services
{
    public class WeekService : IWeekService
    {
        private readonly ApplicationDbContext _context;

        public WeekService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Week>> GetWeeksAsync()
        {
            return await _context.Weeks.ToListAsync();
        }

        public async Task<Week?> GetWeekByIdAsync(int? id)
        {
            return await _context.Weeks.FindAsync(id);
        }

        public async Task<List<Spartan>> GetSpartansAsync()
        {
            return await _context.Spartans.ToListAsync();
        }

        public async Task<Spartan?> GetSpartansByIdAsync(int? id)
        {
            return await _context.Spartans.FindAsync(id);
        }

        public async Task AddWeek(Week week)
        {
            _context.Weeks.Add(week);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task RemoveWeekAsync(Week week)
        {
            _context.Weeks.Remove(week);
            await _context.SaveChangesAsync();
        }
    }
}
