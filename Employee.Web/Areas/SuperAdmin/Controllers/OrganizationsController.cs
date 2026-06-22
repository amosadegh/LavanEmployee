using Employee.DataAccess.Repositories.Interfaces;

using Employee.Entities.Models.Basic;

using Microsoft.AspNetCore.Mvc;

namespace Employee.Web.Areas.SuperAdmin.Controllers

{

    [Area("SuperAdmin")]

    public class OrganizationsController(IOrganizationRepository organizationRepository) : Controller

    {

        private readonly IOrganizationRepository _organizationRepository = organizationRepository;

        // GET: SuperAdmin/Organizations

        public async Task<IActionResult> Index()

        {

            var organizations = await _organizationRepository.GetAsync();

            return View(organizations);

        }

        // GET: SuperAdmin/Organizations/Create

        public IActionResult Create()

        {

            return View();

        }

        // POST: SuperAdmin/Organizations/Create

        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Organization organization)

        {

            if (ModelState.IsValid)

            {

                await _organizationRepository.AddAsync(organization);

                await _organizationRepository.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }

            return View(organization);

        }

        // GET: SuperAdmin/Organizations/Edit/5

        public async Task<IActionResult> Edit(int id)

        {

            var organization = await _organizationRepository.GetSingleAsync(o=>o.OrganizationId == id);

            if (organization == null)

            {

                return NotFound();

            }

            return View(organization);

        }

        // POST: SuperAdmin/Organizations/Edit/5

        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, Organization organization)

        {

            if (id != organization.OrganizationId)

            {

                return NotFound();

            }

            if (ModelState.IsValid)

            {

                _organizationRepository.Update(organization);

                await _organizationRepository.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }

            return View(organization);

        }

        // GET: SuperAdmin/Organizations/Delete/5

        public async Task<IActionResult> Delete(int id)

        {

            var organization = await _organizationRepository.GetSingleAsync(o => o.OrganizationId == id);

            if (organization == null)

            {

                return NotFound();

            }

            return View(organization);

        }

        // POST: SuperAdmin/Organizations/Delete/5

        [HttpPost, ActionName("Delete")]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int id)

        {

            var organization = await _organizationRepository.GetSingleAsync(o => o.OrganizationId == id);

            if (organization != null)

            {

                _organizationRepository.Remove(organization);

                await _organizationRepository.SaveChangesAsync();

            }

            return RedirectToAction(nameof(Index));

        }

    }

}