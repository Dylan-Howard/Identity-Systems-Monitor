using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prognosis.Models;

namespace prognosis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly PrognosisContext _context;

        public JobsController(PrognosisContext context)
        {
            _context = context;
        }

        // GET: api/Jobs
        [HttpGet]
        public async Task<ActionResult<JobList>> GetJob()
        {
            List<Job> jobs = await _context.Jobs.ToListAsync();

            List<Service> services = await _context.Services.ToListAsync();
            Dictionary<Guid, string> serviceMap = [];

            foreach (Service s in services)
            {
                serviceMap.Add(s.ServiceId, s.Name);
            }

            List<JobListItem> jobItems = jobs.Select((j) => new JobListItem {
                JobId = j.JobId,
                ServiceId = j.ServiceId,
                StartDate = j.StartDate,
                NextRunTime = j.NextRunTime,
                Frequency = j.Frequency,
                Active = j.Active,
                ServiceName = serviceMap[j.ServiceId],
            }).ToList();

            return new JobList {
              Jobs = jobItems,
              Total = jobs.Count,
            };
        }

        // GET: api/Jobs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Job>> GetJob(Guid id)
        {
            var Job = await _context.Jobs.FindAsync(id);

            if (Job == null)
            {
                return NotFound();
            }

            return Job;
        }

        // PUT: api/Jobs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJob(Guid id, Job Job)
        {
            if (id != Job.JobId)
            {
                return BadRequest();
            }

            _context.Entry(Job).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Jobs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Job>> PostJob(Job job)
        {
            job.JobId = Guid.NewGuid();
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostJob), new { id = job.JobId }, job);
        }

        // DELETE: api/Jobs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(Guid id)
        {
            var Job = await _context.Jobs.FindAsync(id);
            if (Job == null)
            {
                return NotFound();
            }

            _context.Jobs.Remove(Job);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool JobExists(Guid id)
        {
            return _context.Jobs.Any(e => e.JobId == id);
        }
    }
}
