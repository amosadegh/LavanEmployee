using Employee.DataAccess.Repositories.Implementations;
using Employee.DataAccess.Repositories.Interfaces;
using Employee.Entities.Models.Basic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee.Web.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    public class AdministrationsController(
        IAdministrationRepository administrationRepositoy) : Controller
    {

        private readonly AdministrationRepository _administrationRepositoy = (AdministrationRepository)administrationRepositoy;
        public async Task<IActionResult> Index()
        {
            var administration =await _administrationRepositoy.GetAsync();
            return View(administration);
        }
        // GET: Administrations/Create
        public ActionResult Create()
        {
           
            return View();
        }

        // POST: Administrations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Administration administration)
        {
            if (ModelState.IsValid)
            {
               await _administrationRepositoy.AddAsync(administration);
               await _administrationRepositoy.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(administration);
        }
        // GET: Adminis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administration = await _administrationRepositoy.GetSingleAsync(a=>a.AdministrationId==id);
            if (administration == null)
            {
                return NotFound();
            }
            return View(administration);
        }

        // POST: Adminis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Administration administration)
        {
            if (id != administration.AdministrationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   _administrationRepositoy.Update(administration);
                    await _administrationRepositoy.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_administrationRepositoy.AdministrationExists(administration.AdministrationId))
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
            return View(administration);
        }
    }
}
