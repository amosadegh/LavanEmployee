using Employee.DataAccess.Repositories.Implementations;
using Employee.DataAccess.Repositories.Interfaces;
using Employee.Entities.Models.Basic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee.Web.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    public class StudyLevelsController(IStudyLevelRepository studyLevelRepository) : Controller
    {
        private readonly IStudyLevelRepository _studyLevelRepository=studyLevelRepository;
        public async Task<IActionResult> Index()
        {
            var studyLevels=await _studyLevelRepository.GetAsync();
            return View(studyLevels);
        }
        public ActionResult Create()
        {

            return View();
        }

        // POST: Administrations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(StudyLevel studyLevel)
        {
            if (ModelState.IsValid)
            {
                await _studyLevelRepository.AddAsync(studyLevel);
                await _studyLevelRepository.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(studyLevel);
        }
        // GET: Adminis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studyLevel = await _studyLevelRepository.GetSingleAsync(s=>s.StudyLevelId==id);
            if (studyLevel == null)
            {
                return NotFound();
            }
            return View(studyLevel);
        }

        // POST: Adminis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudyLevel studyLevel)
        {
            if (id != studyLevel.StudyLevelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _studyLevelRepository.Update(studyLevel);
                    await _studyLevelRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_studyLevelRepository.StudyLevelExists(studyLevel.StudyLevelId))
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
            return View(studyLevel);
        }
    }
}
