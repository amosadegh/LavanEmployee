using Employee.DataAccess.Repositories.Interfaces;
using Employee.Entities.Models.Basic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee.Web.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    public class JobTypesController(IJobTypeRepository jobTypeRepository) : Controller
    {
        private readonly IJobTypeRepository _jobTypeRepository = jobTypeRepository;
        public async Task<IActionResult> Index()
        {
            var jobTypes = await _jobTypeRepository.GetAsync();
            return View(jobTypes);
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
        public async Task<ActionResult> Create(JobType jobType)
        {
            if (ModelState.IsValid)
            {
                await _jobTypeRepository.AddAsync(jobType);
                await _jobTypeRepository.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(jobType);
        }
        // GET: Adminis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobType = await _jobTypeRepository.GetSingleAsync(j => j.JobTypeId == id);
            if (jobType == null)
            {
                return NotFound();
            }
            return View(jobType);
        }

        // POST: Adminis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JobType jobType)
        {
            if (id != jobType.JobTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _jobTypeRepository.Update(jobType);
                    await _jobTypeRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_jobTypeRepository.JobTypeExists(jobType.JobTypeId))
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
            return View(jobType);
        }

    }
}
