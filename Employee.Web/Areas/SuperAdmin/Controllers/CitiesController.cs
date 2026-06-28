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
using Employee.DataAccess.Repositories.Implementations;

namespace Employee.Web.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    public class CitiesController(ICityRepository cityRepository,
        IProvinceRepository provinceRepository) : Controller
    {
        private readonly ICityRepository _cityRepository=cityRepository;
        private readonly IProvinceRepository _provinceRepository=provinceRepository;
       

        // GET: SuperAdmin/Cities
        public async Task<IActionResult> Index()
        {
            var cities=await _cityRepository.GetAsync(
                include: c => c.Include(p => p.Province));          
            return View(cities);
        }

       

        // GET: SuperAdmin/Cities/Create
        public async Task<IActionResult> Create()
        {
            var province = await _provinceRepository.GetAsync();
            ViewData["ProvinceId"] = new SelectList(province, "Id", "Name");
            return View();

        }

        // POST: SuperAdmin/Cities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(City city)
        {
            if (ModelState.IsValid)
            {
                await _cityRepository.AddAsync(city);
                await _cityRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var province = await _provinceRepository.GetAsync();
            ViewData["ProvinceId"] = new SelectList(province, "Id", "Name");
            return View(city);

        }

        // GET: SuperAdmin/Cities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _cityRepository.GetSingleAsync(e => e.Id == id);
            if (city == null)
            {
                return NotFound();
            }
            var province = await _provinceRepository.GetAsync();
            ViewData["ProvinceId"] = new SelectList(province, "Id", "Name");
            return View(city);
        }

        // POST: SuperAdmin/Cities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, City city)
        {
            if (id != city.Id)
            {
                return NotFound();
            }
           
            if (ModelState.IsValid)
            {
                try
                {
                    _cityRepository.Update(city);
                    await _cityRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_cityRepository.CityExists(city.Id))
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
            var province = await _provinceRepository.GetAsync();
            ViewData["ProvinceId"] = new SelectList(province, "Id", "Name");
            return View(city);
        }

        // GET: SuperAdmin/Cities/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var city = await _context.Cities
        //        .Include(c => c.Province)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (city == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(city);
        //}

        //// POST: SuperAdmin/Cities/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var city = await _context.Cities.FindAsync(id);
        //    if (city != null)
        //    {
        //        _context.Cities.Remove(city);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool CityExists(int id)
        {
            return _cityRepository.CityExists(id);
        }
    }
}
