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
    public class EduOrientationsController(IEduOrientationRepository eduOrientationRepository
        ,IStudyFieldRepository studyFieldRepository) : Controller
    {
        private readonly IEduOrientationRepository _eduOrientationRepository=eduOrientationRepository;
        private readonly IStudyFieldRepository _studyFieldRepository = studyFieldRepository;
        
        // GET: SuperAdmin/EduOrientations
        public async Task<IActionResult> Index()
        {
            var eduOrientation =await _eduOrientationRepository.GetAsync(
                include: e => e.Include(s => s.StudyField));
           
            return View(eduOrientation);
        }

        // GET: SuperAdmin/EduOrientations/Create
        public async Task<IActionResult> Create()
        {
            var studyfields =await  _studyFieldRepository.GetAsync();
            ViewData["StudyFieldId"] = new SelectList(studyfields, "StudyFieldId", "StudyFieldTitle");            
            return View();
        }

        // POST: SuperAdmin/EduOrientations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EduOrientation eduOrientation)
        {
            if (ModelState.IsValid)
            {
                await _eduOrientationRepository.AddAsync(eduOrientation);
                
                await _eduOrientationRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var studyfields = await _studyFieldRepository.GetAsync();
            ViewData["StudyFieldId"] = new SelectList(studyfields, "StudyFieldId", "StudyFieldTitle");
            return View(eduOrientation);
        }

        // GET: SuperAdmin/EduOrientations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var eduOrientation = await _eduOrientationRepository.GetSingleAsync(e => e.EduOrientationId == id);           
            if (eduOrientation == null)
            {
                return NotFound();
            }
            var studyfields = await _studyFieldRepository.GetAsync();
            ViewData["StudyFieldId"] = new SelectList(studyfields, "StudyFieldId", "StudyFieldTitle");
            return View(eduOrientation);
        }

        // POST: SuperAdmin/EduOrientations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EduOrientation eduOrientation)
        {
            if (id != eduOrientation.EduOrientationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _eduOrientationRepository.Update(eduOrientation);
                  
                    await _eduOrientationRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_eduOrientationRepository.EduOrientationExists(eduOrientation.EduOrientationId))
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
            var studyfields = await _studyFieldRepository.GetAsync();
            ViewData["StudyFieldId"] = new SelectList(studyfields, "StudyFieldId", "StudyFieldTitle");
            return View(eduOrientation);
        }

    //    // GET: SuperAdmin/EduOrientations/Delete/5
    //    public async Task<IActionResult> Delete(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        var eduOrientation = await _context.EduOrientations
    //            .Include(e => e.StudyField)
    //            .FirstOrDefaultAsync(m => m.EduOrientationId == id);
    //        if (eduOrientation == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(eduOrientation);
    //    }

    //    // POST: SuperAdmin/EduOrientations/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> DeleteConfirmed(int id)
    //    {
    //        var eduOrientation = await _context.EduOrientations.FindAsync(id);
    //        if (eduOrientation != null)
    //        {
    //            _context.EduOrientations.Remove(eduOrientation);
    //        }

    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }

    //    private bool EduOrientationExists(int id)
    //    {
    //        return _context.EduOrientations.Any(e => e.EduOrientationId == id);
    //    }
    }
}
