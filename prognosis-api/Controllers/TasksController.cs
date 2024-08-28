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
        public async Task<ActionResult<TaskList>> GetTask(int startIndex, int endIndex, string sort, string order, string? q, bool? active)
        {

            List<Prognosis.Models.Task> tasks = await _context.Tasks.ToListAsync();
            List<Job> jobs = await _context.Jobs.ToListAsync();
            List<Service> services = await _context.Services.ToListAsync();

            List<TaskListItem> taskItems = [];

            for (var i = 0; i < tasks.Count; i++)
            {
                var jobMatch = jobs.Find((j) => j.JobId == tasks[i].JobId);
                if (jobMatch == null)
                {
                    continue;
                }

                var serviceMatch = services.Find((s) => s.ServiceId == jobMatch.ServiceId);
                if (serviceMatch == null)
                {
                    continue;
                }

                taskItems.Add(new TaskListItem {
                    TaskId = tasks[i].TaskId,
                    JobId = tasks[i].TaskId,
                    StartTime = tasks[i].StartTime,
                    EndTime = tasks[i].EndTime,
                    Notes = tasks[i].Notes,
                    Active = tasks[i].Active,
                    ServiceId = serviceMatch.ServiceId,
                    ServiceName = serviceMatch.Name,
                });
            }

            /* Handle filter*/
            if (q != null)
            {
                taskItems = taskItems.FindAll((t) => t.ServiceName.StartsWith(q));
            }
            else if (active != null)
            {
                Console.WriteLine("Finding users by status");
                taskItems = taskItems.FindAll((t) => t.Active == active);
            }

            /* Handle Sorting */
            switch (sort)
            {
                case "serviceName":
                    if (order == "DESC")
                    {
                        taskItems = taskItems.OrderByDescending((p) => p.ServiceName).ToList();
                    }
                    else
                    {
                        taskItems = taskItems.OrderBy((t) => t.ServiceName).ToList();
                    }
                    break;
                case "startTime":
                    if (order == "DESC")
                    {
                        taskItems = taskItems.OrderByDescending((t) => t.StartTime).ToList();
                    }
                    else
                    {
                        taskItems = taskItems.OrderBy((t) => t.StartTime).ToList();
                    }
                    break;
                case "endTime":
                    if (order == "DESC")
                    {
                        taskItems = taskItems.OrderByDescending((t) => t.EndTime).ToList();
                    }
                    else
                    {
                        taskItems = taskItems.OrderBy((t) => t.EndTime).ToList();
                    }
                    break;
                case "notes":
                    if (order == "DESC")
                    {
                        taskItems = taskItems.OrderByDescending((t) => t.Notes).ToList();
                    }
                    else
                    {
                        taskItems = taskItems.OrderBy((t) => t.Notes).ToList();
                    }
                    break;
                case "active":
                    if (order == "DESC")
                    {
                        taskItems = taskItems.OrderByDescending((t) => t.Active).ToList();
                    }
                    else
                    {
                        taskItems = taskItems.OrderBy((t) => t.Active).ToList();
                    }
                    break;
            }

            /* Handle array slicing */
            if (endIndex == 0)
            {
              endIndex = startIndex + 10;
            }
            
            return new TaskList {
              Tasks = taskItems.GetRange(startIndex, Math.Min(endIndex, taskItems.Count) - startIndex),
              Total = taskItems.Count,
            };
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
