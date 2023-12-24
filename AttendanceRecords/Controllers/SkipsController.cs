using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AttendanceRecords.Data;
using AttendanceRecords.Models;
using Microsoft.AspNetCore.Authorization;

namespace AttendanceRecords.Controllers
{
    public class SkipsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SkipsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Skips
        [Authorize(Roles = "Administrator,Guest")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Skip.Include(s => s.Schedule).Include(s => s.Status).Include(s => s.Student);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Skips/Details/5
        [Authorize(Roles = "Administrator,Guest")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Skip == null)
            {
                return NotFound();
            }

            var skip = await _context.Skip
                .Include(s => s.Schedule)
                .Include(s => s.Status)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.SkipId == id);
            if (skip == null)
            {
                return NotFound();
            }

            return View(skip);
        }

        // GET: Skips/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["ScheduleId"] = new SelectList(_context.Schedule, "ScheduleId", "SubjectName");
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "Name");
            ViewData["StudentId"] = new SelectList(_context.Student, "StudentId", "FIO");
            return View();
        }

        // POST: Skips/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SkipId,Date,ScheduleId,StudentId,StatusId")] Skip skip)
        {
            if (ModelState.IsValid)
            {
                _context.Add(skip);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ScheduleId"] = new SelectList(_context.Schedule, "ScheduleId", "SubjectName", skip.ScheduleId);
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "Name", skip.StatusId);
            ViewData["StudentId"] = new SelectList(_context.Student, "StudentId", "FIO", skip.StudentId);
            return View(skip);
        }

        // GET: Skips/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Skip == null)
            {
                return NotFound();
            }

            var skip = await _context.Skip.FindAsync(id);
            if (skip == null)
            {
                return NotFound();
            }
            ViewData["ScheduleId"] = new SelectList(_context.Schedule, "ScheduleId", "SubjectName", skip.ScheduleId);
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "Name", skip.StatusId);
            ViewData["StudentId"] = new SelectList(_context.Student, "StudentId", "FIO", skip.StudentId);
            return View(skip);
        }

        // POST: Skips/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SkipId,Date,ScheduleId,StudentId,StatusId")] Skip skip)
        {
            if (id != skip.SkipId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(skip);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SkipExists(skip.SkipId))
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
            ViewData["ScheduleId"] = new SelectList(_context.Schedule, "ScheduleId", "SubjectName", skip.ScheduleId);
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "Name", skip.StatusId);
            ViewData["StudentId"] = new SelectList(_context.Student, "StudentId", "FIO", skip.StudentId);
            return View(skip);
        }

        // GET: Skips/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Skip == null)
            {
                return NotFound();
            }

            var skip = await _context.Skip
                .Include(s => s.Schedule)
                .Include(s => s.Status)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.SkipId == id);
            if (skip == null)
            {
                return NotFound();
            }

            return View(skip);
        }

        // POST: Skips/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Skip == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Skip'  is null.");
            }
            var skip = await _context.Skip.FindAsync(id);
            if (skip != null)
            {
                _context.Skip.Remove(skip);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SkipExists(int id)
        {
          return (_context.Skip?.Any(e => e.SkipId == id)).GetValueOrDefault();
        }
    }
}
