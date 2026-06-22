using Employee.DataAccess.Repositories.Implementations;
using Employee.DataAccess.Repositories.Interfaces;
using Employee.Entities.Models.Basic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee.Web.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    public class CampsController(ICampRepository campRepository) : Controller
    {
        private readonly ICampRepository _campRepository = campRepository;
        public async Task<IActionResult> IndexAsync()
        {
            var camps = await _campRepository.GetAsync();
            return View(camps);
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
        public async Task<ActionResult> Create(Camp camp)
        {
            if (ModelState.IsValid)
            {
                await _campRepository.AddAsync(camp);
                await _campRepository.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(camp);
        }
        // GET: Adminis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var camp = await _campRepository.GetSingleAsync(j => j.CampId == id);
            if (camp == null)
            {
                return NotFound();
            }
            return View(camp);
        }

        // POST: Adminis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Camp camp)
        {
            if (id != camp.CampId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _campRepository.Update(camp);
                    await _campRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_campRepository.CampExists(camp.CampId))
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
            return View(camp);
        }
    }
}
