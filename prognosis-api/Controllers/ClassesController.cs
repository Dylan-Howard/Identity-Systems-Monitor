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
    public class ClassesController : ControllerBase
    {
        private readonly PrognosisContext _context;

        public ClassesController(PrognosisContext context)
        {
            _context = context;
        }

        // GET: api/Classes
        [HttpGet]
        public async Task<ActionResult<ClassList>> GetClass()
        {
            List<Class> clss = await _context.Classes.ToListAsync();
            return new ClassList {
              Classes = clss,
              Total = clss.Count,
            };
        }

        // GET: api/Classes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Class>> GetClass(Guid id)
        {
            var cls = await _context.Classes.FindAsync(id);

            if (cls == null)
            {
                return NotFound();
            }

            return cls;
        }

        // PUT: api/Classes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClass(Guid id, Class cls)
        {
            if (id != cls.SourcedId)
            {
                return BadRequest();
            }

            _context.Entry(cls).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassExists(id))
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

        // POST: api/Classes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Class>> PostClass(Class cls)
        {
            _context.Classes.Add(cls);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostClass), new { id = Guid.NewGuid() }, cls);
        }

        // DELETE: api/Classes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClass(Guid id)
        {
            var cls = await _context.Classes.FindAsync(id);
            if (cls == null)
            {
                return NotFound();
            }

            _context.Classes.Remove(cls);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClassExists(Guid id)
        {
            return _context.Classes.Any(e => e.SourcedId == id);
        }
    }
}
