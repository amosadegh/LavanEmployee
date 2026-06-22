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
    public class ContractTypesController(IContractTypeRepository contractTypeRepository) : Controller
    {
        private readonly IContractTypeRepository _contractTypeRepository = (ContractTypeRepository)contractTypeRepository;

        // GET: SuperAdmin/ContractTypes
        public async Task<IActionResult> Index()
        {
           var contractType = _contractTypeRepository.GetAsync();
            return View(await contractType); 
        }

        // GET: SuperAdmin/ContractTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contractType = await _contractTypeRepository.GetSingleAsync(c=>c.ContractTypeId==id);
            if (contractType == null)
            {
                return NotFound();
            }

            return View(contractType);
        }

        // GET: SuperAdmin/ContractTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SuperAdmin/ContractTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContractTypeId,ContractTypeTitle")] ContractType contractType)
        {
            if (ModelState.IsValid)
            {
                await _contractTypeRepository.AddAsync(contractType);
                await _contractTypeRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contractType);
        }

        // GET: SuperAdmin/ContractTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contractType = await _contractTypeRepository.GetSingleAsync(c=>c.ContractTypeId==id);
            if (contractType == null)
            {
                return NotFound();
            }
            return View(contractType);
        }

        // POST: SuperAdmin/ContractTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ContractType contractType)
        {
            if (id != contractType.ContractTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _contractTypeRepository.Update(contractType);
                    await _contractTypeRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_contractTypeRepository.ContractTypeExists(contractType.ContractTypeId))
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
            return View(contractType);
        }

        // GET: SuperAdmin/ContractTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contractType = await _contractTypeRepository.GetSingleAsync(c=>c.ContractTypeId==id);
            if (contractType == null)
            {
                return NotFound();
            }

            return View(contractType);
        }

        // POST: SuperAdmin/ContractTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contractType = await _contractTypeRepository.GetSingleAsync(c=>c.ContractTypeId==id);
            if (contractType != null)
            {
               _contractTypeRepository.Remove(contractType);
            }

            await _contractTypeRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

       
    }
}
