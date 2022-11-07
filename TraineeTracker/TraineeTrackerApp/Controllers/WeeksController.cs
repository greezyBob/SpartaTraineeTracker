﻿using System;
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
    public class WeeksController : Controller
    {
        private readonly IWeekService _service;
        private UserManager<Spartan> _userManager;

        public WeeksController(IWeekService service, UserManager<Spartan> userManager)
        {
            _service = service;
            _userManager = userManager;
        }


        // GET: Weeks
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var weeks = await _service.GetWeeksAsync();
            var filteredWeeks = weeks.Where(w => w.SpartanId == currentUser.Id)
                .OrderBy(w => w.WeekStart.Date).ToList();
            return View(filteredWeeks);
        }

        // GET: Weeks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _service.GetWeeksAsync().Result == new List<Week>())
            {
                return NotFound();
            }

            var week = await _service.GetWeekByIdAsync(id);
            if (week == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            if (!WeekBelongsToUser(week, currentUser))
            {
                return Forbid();
            }

            return View(week);
        }

        // GET: Weeks/Create
        [Authorize(Roles = "Trainee")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Weeks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize(Roles = "Trainee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Start,Stop,Continue,WeekStart,GitHubLink,TechnicalSkill,ConsultantSkill")] Week week)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            week.SpartanId = currentUser.Id;
            week.Spartan = currentUser;

            if (week.SpartanId != null)
            {
                await _service.AddWeek(week);
                return RedirectToAction(nameof(Index));
            }

            return View(week);
        }

        // GET: Weeks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _service.GetWeeksAsync().Result == new List<Week>())
            {
                return NotFound();
            }

            var week = await _service.GetWeekByIdAsync(id);
            if (week == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            if (!WeekBelongsToUser(week, currentUser))
            {
                return Forbid();
            }

            return View(week);
        }

        // POST: Weeks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Start,Stop,Continue,WeekStart,GitHubLink,TechnicalSkill,ConsultantSkill")] Week week)
        {
            if (id != week.Id)
            {
                return NotFound();
            }

            try
            {
                var weekToUpdate = _service.GetWeekByIdAsync(id).Result;
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                if (!WeekBelongsToUser(weekToUpdate, currentUser)) return Forbid();;
                weekToUpdate.WeekStart = week.WeekStart;
                weekToUpdate.Start = week.Start ?? weekToUpdate.Start;
                weekToUpdate.Stop = week.Stop ?? weekToUpdate.Stop;
                weekToUpdate.Continue = week.Continue ?? weekToUpdate.Continue;
                weekToUpdate.GitHubLink = week.GitHubLink ?? weekToUpdate.GitHubLink;
                weekToUpdate.TechnicalSkill = week.TechnicalSkill;
                weekToUpdate.ConsultantSkill = week.ConsultantSkill;

                await _service.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeekExists(week.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Details), new {id = week.Id});
        }

        // POST: Weeks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_service.GetWeeksAsync().Result == new List<Week>())
            {
                return Problem("Entity set 'ApplicationDbContext.Weeks' is empty.");
            }

            
            var week = await _service.GetWeekByIdAsync(id);
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (!WeekBelongsToUser(week, currentUser))
            {
                return Forbid();
            }
            if (week != null)
            {
                await _service.RemoveWeekAsync(week);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool WeekExists(int id)
        {
            return _service.GetWeekByIdAsync(id).Result != null;
        }

        private bool WeekBelongsToUser(Week week, Spartan user)
        {
            return week.SpartanId == user.Id;
        }
    }
}
