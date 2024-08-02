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
    public class OrganizationsController : ControllerBase
    {
        private readonly PrognosisContext _context;

        public OrganizationsController(PrognosisContext context)
        {
            _context = context;
        }

        // GET: api/Organizations
        [HttpGet]
        public async Task<ActionResult<OrgList>> GetOrganization(int startIndex, int endIndex, string sort, string order, string? q)
        {
            List<Org> orgs = await _context.Orgs.ToListAsync();

            if (q != null)
            {
                orgs = orgs.FindAll((o) => o.Name.ToLower().Contains(q.ToLower()));
            }

            /* Handle Sorting */
            switch (sort)
            {
                case "name":
                    if (order == "DESC")
                    {
                        orgs = orgs.OrderByDescending((o) => o.Name).ToList();
                    }
                    else
                    {
                        orgs = orgs.OrderBy((o) => o.Name).ToList();
                    }
                    break;
                case "status":
                    if (order == "DESC")
                    {
                        orgs = orgs.OrderByDescending((o) => o.Status).ToList();
                    }
                    else
                    {
                        orgs = orgs.OrderBy((o) => o.Status).ToList();
                    }
                    break;
                case "dateLastModified":
                    if (order == "DESC")
                    {
                        orgs = orgs.OrderByDescending((o) => o.DateLastModified).ToList();
                    }
                    else
                    {
                        orgs = orgs.OrderBy((o) => o.DateLastModified).ToList();
                    }
                    break;
                case "type":
                    if (order == "DESC")
                    {
                        orgs = orgs.OrderByDescending((o) => o.Type).ToList();
                    }
                    else
                    {
                        orgs = orgs.OrderBy((o) => o.Type).ToList();
                    }
                    break;
            }

            /* Handle array slicing */
            if (endIndex == 0)
            {
              endIndex = startIndex + 10;
            }

            return new OrgList {
              Orgs = orgs.GetRange(startIndex, Math.Min(endIndex, orgs.Count) - startIndex),
              Total = orgs.Count,
            };
        }

        // GET: api/Organizations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrgShow>> GetOrganization(Guid id)
        {
            var org = await _context.Orgs.FindAsync(id);

            if (org == null)
            {
                return NotFound();
            }

            List<Class> orgClasses = await _context.Classes.ToListAsync();
            orgClasses = orgClasses.FindAll((c) => c.OrgSourcedId == org.SourcedId);

            return new OrgShow {
                SourcedId = org.SourcedId,
                Status = org.Status,
                DateLastModified = org.DateLastModified,
                Name = org.Name,
                Identifier = org.Identifier,
                Type = org.Type,
                Address = org.Address,
                City = org.City,
                State = org.State,
                Zip = org.Zip,
                Classes = orgClasses,
            };
        }

        // PUT: api/Organizations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrganization(Guid id, Org org)
        {
            if (id != org.SourcedId)
            {
                return BadRequest();
            }

            _context.Entry(org).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizationExists(id))
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

        // POST: api/Organizations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Org>> PostOrganization(Org org)
        {
            _context.Orgs.Add(org);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostOrganization), new { id = Guid.NewGuid() }, org);
        }

        // DELETE: api/Organizations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrganization(Guid id)
        {
            var org = await _context.Orgs.FindAsync(id);
            if (org == null)
            {
                return NotFound();
            }

            _context.Orgs.Remove(org);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrganizationExists(Guid id)
        {
            return _context.Orgs.Any(e => e.SourcedId == id);
        }
    }
}
