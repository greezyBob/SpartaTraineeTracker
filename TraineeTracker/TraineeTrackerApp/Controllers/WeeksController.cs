using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TraineeTrackerApp.Data;
using TraineeTrackerApp.Models;

namespace TraineeTrackerApp.Controllers
{
    public class WeeksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WeeksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Weeks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Weeks.Include(w => w.Spartan);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Weeks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Weeks == null)
            {
                return NotFound();
            }

            var week = await _context.Weeks
                .Include(w => w.Spartan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (week == null)
            {
                return NotFound();
            }

            return View(week);
        }

        // GET: Weeks/Create
        public IActionResult Create()
        {
            ViewData["SpartanId"] = new SelectList(_context.Spartans, "Id", "Id");
            return View();
        }

        // POST: Weeks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Start,Stop,Continue,WeekStart,GitHubLink,TechnicalSkill,ConsultantSkill,SpartanId")] Week week)
        {
            if (ModelState.IsValid)
            {
                _context.Add(week);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpartanId"] = new SelectList(_context.Spartans, "Id", "Id", week.SpartanId);
            return View(week);
        }

        // GET: Weeks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Weeks == null)
            {
                return NotFound();
            }

            var week = await _context.Weeks.FindAsync(id);
            if (week == null)
            {
                return NotFound();
            }
            ViewData["SpartanId"] = new SelectList(_context.Spartans, "Id", "Id", week.SpartanId);
            return View(week);
        }

        // POST: Weeks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Start,Stop,Continue,WeekStart,GitHubLink,TechnicalSkill,ConsultantSkill,SpartanId")] Week week)
        {
            if (id != week.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(week);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpartanId"] = new SelectList(_context.Spartans, "Id", "Id", week.SpartanId);
            return View(week);
        }

        // GET: Weeks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Weeks == null)
            {
                return NotFound();
            }

            var week = await _context.Weeks
                .Include(w => w.Spartan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (week == null)
            {
                return NotFound();
            }

            return View(week);
        }

        // POST: Weeks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Weeks == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Weeks'  is null.");
            }
            var week = await _context.Weeks.FindAsync(id);
            if (week != null)
            {
                _context.Weeks.Remove(week);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WeekExists(int id)
        {
          return _context.Weeks.Any(e => e.Id == id);
        }
    }
}
