using System.Security.Claims;
using TraineeTrackerApp.Models;

namespace TraineeTrackerApp.Services
{
    public interface IWeekServiceUserFunctions
    {
        public Task<Spartan> GetUserAsync(ClaimsPrincipal principal);
    }
}
