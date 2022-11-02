using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TraineeTrackerApp.Models;

namespace TraineeTrackerApp.Services
{
    public class WeekServiceUserFunctions : IWeekServiceUserFunctions
    {
        private readonly UserManager<Spartan> _userManager;

        public WeekServiceUserFunctions(UserManager<Spartan> userManager)
        {
            _userManager = userManager;
        }

        public Task<Spartan> GetUserAsync(ClaimsPrincipal principal)
        {
            var id = _userManager.GetUserId(principal);
            return _userManager.FindByIdAsync(id);
        }

    }
}
