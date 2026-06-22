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
    public class SecondOrganizationsController(
        ISecondOrganizationRepository secondOrganizationRepository,
        IOrganizationRepository organizationRepository) : Controller
    {
       
        private readonly ISecondOrganizationRepository _secondOrganizationRepository = secondOrganizationRepository;
        private readonly IOrganizationRepository _organizationRepository = organizationRepository;
        // GET: SuperAdmin/SecondOrganizations
        public async Task<IActionResult> Index()
        {
            var secondOrganizations =await _secondOrganizationRepository.GetAsync(
                include: s => s.Include(o => o.Organization));
           // var applicationDbContext = _context.SecondOrganizations.Include(s => s.Organization);
            return View(secondOrganizations);
        }

     

        // GET: SuperAdmin/SecondOrganizations/Create
        public async Task<IActionResult> Create()
        {
            var organizations = await _organizationRepository.GetAsync();
            ViewData["OrganizationId"] = new SelectList(organizations, "OrganizationId", "OrganizationTitle");
            return View();
        }

        // POST: SuperAdmin/SecondOrganizations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SecondOrganization secondOrganization)
        {
            if (ModelState.IsValid)
            {
                await _secondOrganizationRepository.AddAsync(secondOrganization);
                await _secondOrganizationRepository.SaveChangesAsync();               
                return RedirectToAction(nameof(Index));
            }
            var organizations = await _organizationRepository.GetAsync();
            ViewData["OrganizationId"] = new SelectList(organizations, "OrganizationId", "OrganizationTitle", secondOrganization.OrganizationId);
            return View(secondOrganization);
        }

        // GET: SuperAdmin/SecondOrganizations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var secondOrganization = await _secondOrganizationRepository.GetSingleAsync(s=>s.SecondOrganizationId==id);
            if (secondOrganization == null)
            {
                return NotFound();
            }
            var organizations = await _organizationRepository.GetAsync();
            ViewData["OrganizationId"] = new SelectList(organizations, "OrganizationId", "OrganizationTitle", secondOrganization.OrganizationId);
            return View(secondOrganization);
        }

        // POST: SuperAdmin/SecondOrganizations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SecondOrganization secondOrganization)
        {
            if (id != secondOrganization.SecondOrganizationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _secondOrganizationRepository.Update(secondOrganization);
                    await _secondOrganizationRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_secondOrganizationRepository.SecondOrganizationExists(secondOrganization.SecondOrganizationId))
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
            var organizations = await _organizationRepository.GetAsync();
            ViewData["OrganizationId"] = new SelectList(organizations, "OrganizationId", "OrganizationTitle", secondOrganization.OrganizationId);
            return View(secondOrganization);
        }

        // GET: SuperAdmin/SecondOrganizations/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var secondOrganization = await _context.SecondOrganizations
        //        .Include(s => s.Organization)
        //        .FirstOrDefaultAsync(m => m.SecondOrganizationId == id);
        //    if (secondOrganization == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(secondOrganization);
        //}

        //// POST: SuperAdmin/SecondOrganizations/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var secondOrganization = await _context.SecondOrganizations.FindAsync(id);
        //    if (secondOrganization != null)
        //    {
        //        _context.SecondOrganizations.Remove(secondOrganization);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool SecondOrganizationExists(int id)
        //{
        //    return _context.SecondOrganizations.Any(e => e.SecondOrganizationId == id);
        //}
    }
}
