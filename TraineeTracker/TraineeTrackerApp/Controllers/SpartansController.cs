using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TraineeTrackerApp.Models;
using TraineeTrackerApp.Services;

namespace TraineeTrackerApp.Controllers;

public class SpartansController : Controller
{
    private readonly ITraineeService _traineeService;
    private readonly IWeekService _weekService;
    private UserManager<Spartan> _userManager;

    public SpartansController(ITraineeService traineeService, IWeekService weekService, UserManager<Spartan> userManager)
    {
        _traineeService = traineeService;
        _weekService = weekService; 
        _userManager = userManager; 
    }

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


    [Authorize(Roles = "Admin")]
    // POST: Delete/Trainees/{id}
    public async Task<IActionResult> Delete(string? id)
    {
        if (id == null || _traineeService.GetSpartansAsync().Result == new List<Spartan>())
        {
            return NotFound();
        }

        var spartan = await _traineeService.GetSpartanByIdAsync(id);
        if (spartan == null)
        {
            return NotFound();
        }

        return View(spartan);
    }

    // POST: Delete/Trainees/{id}
    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string? id)
    {
        if (_traineeService.GetSpartansAsync().Result == new List<Spartan>())
        {
            return Problem("Entity set 'ApplicationDbContext.Spartans' is empty.");
        }


        var spartan = await _traineeService.GetSpartanByIdAsync(id);
        var currentUser = await _userManager.GetUserAsync(HttpContext.User);
        if (spartan.Id == currentUser.Id)
        {
            return Unauthorized();
        }
        if (spartan != null)
        {
            await _traineeService.RemoveSpartanAsync(spartan);
        }

        return RedirectToAction(nameof(Index));
    }
}