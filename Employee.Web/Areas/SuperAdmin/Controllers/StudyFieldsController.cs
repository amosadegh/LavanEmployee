using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Employee.DataAccess.Data;
using Employee.Entities.Models.Basic;
using Employee.DataAccess.Repositories.Interfaces;

namespace Employee.Web.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    public class StudyFieldsController(IStudyFieldRepository studyFieldRepository) : Controller
    {
        private readonly IStudyFieldRepository _studyFieldRepository=studyFieldRepository;

        // GET: SuperAdmin/StudyFields
        public async Task<IActionResult> Index()
        {
            return View(await _studyFieldRepository.GetAsync());
        }

        // GET: SuperAdmin/StudyFields/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SuperAdmin/StudyFields/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudyField studyField)
        {
            if (ModelState.IsValid)
            {
                await _studyFieldRepository.AddAsync(studyField);
                await _studyFieldRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(studyField);
        }

        // GET: SuperAdmin/StudyFields/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studyField = await _studyFieldRepository.GetSingleAsync(s=>s.StudyFieldId==id);
            if (studyField == null)
            {
                return NotFound();
            }
            return View(studyField);
        }

        // POST: SuperAdmin/StudyFields/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,StudyField studyField)
        {
            if (id != studyField.StudyFieldId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _studyFieldRepository.Update(studyField);
                  
                    await _studyFieldRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_studyFieldRepository.StudyFieldExists(studyField.StudyFieldId))
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
            return View(studyField);
        }

        // GET: SuperAdmin/StudyFields/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var studyField = await _context.StudyFields
        //        .FirstOrDefaultAsync(m => m.StudyFieldId == id);
        //    if (studyField == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(studyField);
        //}

        //// POST: SuperAdmin/StudyFields/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var studyField = await _context.StudyFields.FindAsync(id);
        //    if (studyField != null)
        //    {
        //        _context.StudyFields.Remove(studyField);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

  
    }
}
