using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TraineeTrackerApp.Data;
using TraineeTrackerApp.Models;
using TraineeTrackerApp.Services;
using TraineeTrackerApp.Utilities;

namespace TraineeTrackerApp.Controllers;

[Authorize(Roles = "Admin")]
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

    
    public async Task<IActionResult> Index()
    {
        var spartans = await _traineeService.GetSpartansAsync();
        //var filteredWeeks = weeks.Where(w => w.Email == currentUser.Id)
        //    .OrderBy(w => w.Email).ToList();
        return View(spartans);
    }

    // GET: Trainees/Details/{id}
    public async Task<IActionResult> Details(string? id)
    {
        if (id == null || _traineeService.GetSpartansAsync().Result == new List<Spartan>())
        {
            return NotFound();
        }
;
        var spartan = await _traineeService.GetSpartanByIdAsync(id);
        if (spartan == null)
        {
            return NotFound();
        }

        return View(spartan);
    }


    
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

    public async Task<IActionResult> Edit(string? id)
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Spartan spartanToUpdate)
    {
        var role = Request.Form["roles"];
        var currentRole = await _userManager.GetRolesAsync(spartanToUpdate);
        await _userManager.RemoveFromRoleAsync(spartanToUpdate, currentRole[0]);
        await _userManager.AddToRoleAsync(spartanToUpdate, role);
        await _traineeService.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}