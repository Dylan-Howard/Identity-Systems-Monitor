using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prognosis.Models;

namespace prognosis.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EnrollmentsController : ControllerBase
{
    private readonly PrognosisContext _context;

    public EnrollmentsController(PrognosisContext context)
    {
        _context = context;
    }

    // GET: api/Enrollments
    [HttpGet]
    public async Task<ActionResult<EnrollmentList>> GetEnrollment(Guid? userSourcedId, Guid? classSourcedId)
    {
        List<Enrollment> enrollments = await _context.Enrollments.ToListAsync();

        if (userSourcedId != null && userSourcedId != Guid.Empty)
        {
            enrollments = enrollments.FindAll((e) => e.UserSourcedId == userSourcedId);
        }

        if (classSourcedId != null && classSourcedId != Guid.Empty)
        {
            enrollments = enrollments.FindAll((e) => e.ClassSourcedId == classSourcedId);
        }

        return new EnrollmentList {
            Enrollments = enrollments,
            Total = enrollments.Count,
        };
    }

    // GET: api/Enrollments/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Enrollment>> GetEnrollment(Guid id)
    {
        var enrollment = await _context.Enrollments.FindAsync(id);

        if (enrollment == null)
        {
            return NotFound();
        }

        return enrollment;
    }

    // PUT: api/Enrollments/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEnrollment(Guid id, Enrollment enrollment)
    {
        if (id != enrollment.SourcedId)
        {
            return BadRequest();
        }

        _context.Entry(enrollment).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EnrollmentExists(id))
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

    // POST: api/Enrollments
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Enrollment>> PostEnrollment(Enrollment enrollment)
    {
        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(PostEnrollment), new { id = Guid.NewGuid() }, enrollment);
    }

    // DELETE: api/Enrollments/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEnrollment(Guid id)
    {
        var enrollment = await _context.Enrollments.FindAsync(id);
        if (enrollment == null)
        {
            return NotFound();
        }

        _context.Enrollments.Remove(enrollment);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool EnrollmentExists(Guid id)
    {
        return _context.Enrollments.Any(e => e.SourcedId == id);
    }
}
