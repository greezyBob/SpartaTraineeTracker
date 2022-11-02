using Microsoft.EntityFrameworkCore;
using TraineeTrackerApp.Data;
using TraineeTrackerApp.Models;

namespace TraineeTrackerApp.Services
{
    public class TraineeService : ITraineeService
    {
        private readonly ApplicationDbContext _context;

        public TraineeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Spartan?> GetSpartanByIdAsync(int? id)
        {
            return await _context.Spartans.FindAsync(id);
        }

        public async Task<List<Spartan>> GetSpartansAsync()
        {
            return await _context.Spartans.ToListAsync();
        }
    }
}
