using Employee.DataAccess.Repositories.Interfaces;
using Employee.Entities.Models.Basic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee.Web.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    public class BanksController(IBankRepository bankRepository) : Controller
    {
        private readonly IBankRepository _bankRepository = bankRepository;
        public async Task<IActionResult> Index()
        {
            var banks = await _bankRepository.GetAsync();
            return View(banks);
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
        public async Task<ActionResult> Create(Bank bank)
        {
            if (ModelState.IsValid)
            {
                await _bankRepository.AddAsync(bank);
                await _bankRepository.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(bank);
        }
        // GET: Adminis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bank = await _bankRepository.GetSingleAsync(j => j.BankId == id);
            if (bank == null)
            {
                return NotFound();
            }
            return View(bank);
        }

        // POST: Adminis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Bank bank)
        {
            if (id != bank.BankId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _bankRepository.Update(bank);
                    await _bankRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_bankRepository.BankExists(bank.BankId))
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
            return View(bank);
        }

    }
}
