using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ComplexModelBinding.Data;
using ComplexModelBinding.Models;
using System.Data.Common;

namespace ComplexModelBinding.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly IInstructorRepository _instructorRepo;
        public InstructorsController(IInstructorRepository instructorRepo)
        {
            _instructorRepo = instructorRepo;
        }

        // GET: Instructors
        public IActionResult Index()
        {
            return View(_instructorRepo.GetAllInstructors());
        }

        // GET: Instructors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _instructorRepo.Instructors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: Instructors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Instructors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _instructorRepo.Add(instructor);
                await _instructorRepo.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(instructor);
        }

        // GET: Instructors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _instructorRepo.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }

        // POST: Instructors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName")] Instructor instructor)
        {
            if (id != instructor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _instructorRepo.Update(instructor);
                    await _instructorRepo.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(instructor.Id))
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
            return View(instructor);
        }

        // GET: Instructors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _instructorRepo.Instructors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _instructorRepo.Instructors.FindAsync(id);



            // change all related courses to a null instructor
            using DbConnection con = _instructorRepo.Database.GetDbConnection();
            await con.OpenAsync();
            using DbCommand query = con.CreateCommand();
            query.CommandText = "UPDATE Courses SET TeacherId = null WHERE TeacherId = " + instructor.Id;
            int rowsAffected = await query.ExecuteNonQueryAsync();

            TempData["Message"] = $"{instructor.FullName} was removed from {rowsAffected} courses.";

            _instructorRepo.Instructors.Remove(instructor);
            await _instructorRepo.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstructorExists(int id)
        {
            return _instructorRepo.Instructors.Any(e => e.Id == id);
        }
    }
}
