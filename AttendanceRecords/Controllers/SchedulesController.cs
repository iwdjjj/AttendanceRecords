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
    public class SchedulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SchedulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Schedules
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Schedule.Include(s => s.Group).Include(s => s.Subject).Include(s => s.Teacher);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Schedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Schedule == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule
                .Include(s => s.Group)
                .Include(s => s.Subject)
                .Include(s => s.Teacher)
                .FirstOrDefaultAsync(m => m.ScheduleId == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // GET: Schedules/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["GroupId"] = new SelectList(_context.Group, "GroupId", "GroupName");
            ViewData["SubjectId"] = new SelectList(_context.Subject, "SubjectId", "Name");
            ViewData["TeacherId"] = new SelectList(_context.Teacher, "TeacherId", "FIO");
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScheduleId,Date,SubjectId,GroupId,TeacherId")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupId"] = new SelectList(_context.Group, "GroupId", "GroupName", schedule.GroupId);
            ViewData["SubjectId"] = new SelectList(_context.Subject, "SubjectId", "Name", schedule.SubjectId);
            ViewData["TeacherId"] = new SelectList(_context.Teacher, "TeacherId", "FIO", schedule.TeacherId);
            return View(schedule);
        }

        // GET: Schedules/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Schedule == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = new SelectList(_context.Group, "GroupId", "GroupName", schedule.GroupId);
            ViewData["SubjectId"] = new SelectList(_context.Subject, "SubjectId", "Name", schedule.SubjectId);
            ViewData["TeacherId"] = new SelectList(_context.Teacher, "TeacherId", "FIO", schedule.TeacherId);
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ScheduleId,Date,SubjectId,GroupId,TeacherId")] Schedule schedule)
        {
            if (id != schedule.ScheduleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(schedule.ScheduleId))
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
            ViewData["GroupId"] = new SelectList(_context.Group, "GroupId", "GroupName", schedule.GroupId);
            ViewData["SubjectId"] = new SelectList(_context.Subject, "SubjectId", "Name", schedule.SubjectId);
            ViewData["TeacherId"] = new SelectList(_context.Teacher, "TeacherId", "FIO", schedule.TeacherId);
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Schedule == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule
                .Include(s => s.Group)
                .Include(s => s.Subject)
                .Include(s => s.Teacher)
                .FirstOrDefaultAsync(m => m.ScheduleId == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Schedule == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Schedule'  is null.");
            }
            var schedule = await _context.Schedule.FindAsync(id);
            if (schedule != null)
            {
                _context.Schedule.Remove(schedule);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(int id)
        {
          return (_context.Schedule?.Any(e => e.ScheduleId == id)).GetValueOrDefault();
        }
    }
}
