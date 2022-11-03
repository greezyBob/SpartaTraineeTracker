using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TraineeTrackerApp.Data;
using TraineeTrackerApp.Models;
using TraineeTrackerApp.Services;

namespace TraineeTrackerApp.Controllers
{
    [Authorize]
    public class TraineesController : Controller
    {
        private readonly ITraineeService _traineeService;
        private readonly IWeekService _weekService;
        private UserManager<Spartan> _userManager;

        public TraineesController(ITraineeService traineeService, IWeekService weekService, UserManager<Spartan> userManager)
        {
            _traineeService = traineeService;
            _weekService = weekService;
            _userManager = userManager;
        }


        // GET: Trainees

        [Authorize(Roles = "Trainer, Admin")]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            //var applicationDbContext = _service.Weeks.Include(w => w.Spartan);
            //return View(await applicationDbContext.ToListAsync());
            var spartans = await _traineeService.GetSpartansAsync();
            //var filteredWeeks = weeks.Where(w => w.Email == currentUser.Id)
            //    .OrderBy(w => w.Email).ToList();
            return View(spartans);
        }

        // GET: Trainees/Details/{id}
        [Authorize(Roles = "Trainer, Admin")]
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _traineeService.GetSpartansAsync().Result == new List<Spartan>())
            {
                return NotFound();
            }

            //var week = await _service.Weeks
            //    .Include(w => w.Spartan)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var spartan = await _traineeService.GetSpartanByIdAsync(id);
            if (spartan == null)
            {
                return NotFound();
            }

            return View(spartan);
        }

        // GET: Trainees/{TraineeId}/Weeks
        public async Task<IActionResult> Tracker(string? id)
        {
            var weeks = await _traineeService.GetWeeksBySpartanIdAsync(id);

            //var filteredWeeks = weeks.Where(w => w.SpartanId == id)
            //    .OrderBy(w => w.WeekStart).ToList();

            return View(weeks);
        }

        // GET: Trainees/{TraineeId}/Weeks/{weekId}
        public async Task<IActionResult> WeekDetails(int? id)
        {
            if (id == null || _weekService.GetWeeksAsync().Result == new List<Week>())
            {
                return NotFound();
            }

            //var week = await _service.Weeks
            //    .Include(w => w.Spartan)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var week = await _weekService.GetWeekByIdAsync(id);
            if (week == null)
            {
                return NotFound();
            }

            return View(week);
        }

    }
}
