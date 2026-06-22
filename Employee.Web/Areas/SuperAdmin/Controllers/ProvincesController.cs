using Employee.DataAccess.Repositories.Implementations;
using Employee.DataAccess.Repositories.Interfaces;
using Employee.Entities.Models.Basic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee.Web.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    public class ProvincesController(IProvinceRepository provinceRepository) : Controller
    {
        private readonly IProvinceRepository _provinceRepository = provinceRepository;
        public async Task<IActionResult> Index()
        {
            var jobs = await _provinceRepository.GetAsync();
            return View(jobs);
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
        public async Task<ActionResult> Create(Province province)
        {
            if (ModelState.IsValid)
            {
                await _provinceRepository.AddAsync(province);
                await _provinceRepository.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(province);
        }
        // GET: Adminis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var province = await _provinceRepository.GetSingleAsync(j => j.Id == id);
            if (province == null)
            {
                return NotFound();
            }
            return View(province);
        }

        // POST: Adminis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Province province)
        {
            if (id != province.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _provinceRepository.Update(province);
                    await _provinceRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_provinceRepository.ProvinceExists(province.Id))
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
            return View(province);
        }

    }
}
