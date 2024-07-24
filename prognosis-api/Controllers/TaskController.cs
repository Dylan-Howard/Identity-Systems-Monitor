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
    public class TasksController : ControllerBase
    {
        private readonly PrognosisContext _context;

        public TasksController(PrognosisContext context)
        {
            _context = context;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prognosis.Models.Task>>> GetTask()
        {
            return await _context.Tasks.ToListAsync();
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Prognosis.Models.Task>> GetTask(Guid id)
        {
            var Task = await _context.Tasks.FindAsync(id);

            if (Task == null)
            {
                return NotFound();
            }

            return Task;
        }

        // PUT: api/Tasks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(Guid id, Prognosis.Models.Task Task)
        {
            if (id != Task.TaskId)
            {
                return BadRequest();
            }

            _context.Entry(Task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
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

        // POST: api/Tasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Prognosis.Models.Task>> PostTask(Prognosis.Models.Task Task)
        {
            _context.Tasks.Add(Task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostTask), new { id = Guid.NewGuid() }, Task);
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var Task = await _context.Tasks.FindAsync(id);
            if (Task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(Task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskExists(Guid id)
        {
            return _context.Tasks.Any(e => e.TaskId == id);
        }
    }
}
