using Employee.DataAccess.Repositories.Interfaces;
using Employee.Entities.Models.Basic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee.Web.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]

    public class JobLOcationsController(IJobLocationRepository jobLocationRepository) : Controller
    {
        private readonly IJobLocationRepository _jobLocationRepository = jobLocationRepository;
        public async Task<IActionResult> Index()
        {
            var joblocations = await _jobLocationRepository.GetAsync();
            return View(joblocations);
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
        public async Task<ActionResult> Create(JobLocation jobLocation)
        {
            if (ModelState.IsValid)
            {
                await _jobLocationRepository.AddAsync(jobLocation);
                await _jobLocationRepository.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(jobLocation);
        }
        // GET: Adminis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobLocation = await _jobLocationRepository.GetSingleAsync(j => j.JobLocId == id);
            if (jobLocation == null)
            {
                return NotFound();
            }
            return View(jobLocation);
        }

        // POST: Adminis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JobLocation jobLocation)
        {
            if (id != jobLocation.JobLocId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _jobLocationRepository.Update(jobLocation);
                    await _jobLocationRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_jobLocationRepository.JobLocationEists(jobLocation.JobLocId))
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
            return View(jobLocation);
        }

    }
}

