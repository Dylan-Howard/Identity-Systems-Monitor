using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;
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

        private async Task<int> GetEnrollmentCount(Class cls)
        {
            List<Enrollment> enrollments = await _context.Enrollments
                .Where((e) => e.ClassSourcedId == cls.SourcedId)
                .ToListAsync();

            return enrollments.Count;
        }

        private async Task<IEnumerable<ClassEnrollment>> GetClassEnrollments(Class cls)
        {
            List<ClassEnrollment> classEnrollments = [];

            /* Get One Roster Service */
            Service? oneRosterService = await _context.Services.FirstOrDefaultAsync((s) => s.Name == "One Roster");
            if (oneRosterService == null)
            {
                return [];
            }

            /* Get One Roster Users */
            List<Link> links = await _context.Links.Where((usr) => usr.ServiceId == oneRosterService.ServiceId).ToListAsync();
            if (links.Count == 0)
            {
                return [];
            }

            Dictionary<Guid, string> userMap = [];
            foreach (Link l in links)
            {
                userMap.Add(l.LinkId, l.Email ?? "");
            }

            /* Get One Roster Enrollments */
            List<Enrollment> enrollments = await _context.Enrollments.Where((erl) => erl.ClassSourcedId == cls.SourcedId).ToListAsync();

            return enrollments.Select((erl) => new ClassEnrollment {
                UserSourcedId = erl.UserSourcedId,
                Username = userMap[erl.UserSourcedId],
                Role = erl.Role,
                Primary = erl.Primary,
                BeginDate = erl.BeginDate,
                EndDate = erl.EndDate,
            });
        }

        // GET: api/Classes
        [HttpGet]
        public async Task<ActionResult<ClassList>> GetClass(int startIndex, int endIndex, string? sort, string? order, string? q, Guid? orgSourcedId)
        {
            List<Class> classes = await _context.Classes.ToListAsync();

            /* Handle filter*/
            if (q != null)
            {
                classes = classes.FindAll((c) => c.Title.ToLower().StartsWith(q.ToLower()));
            }
            if (orgSourcedId != null && orgSourcedId != Guid.Empty)
            {
                classes = classes.FindAll((c) => c.OrgSourcedId == orgSourcedId);
            }

            /* Handle array slicing */
            if (endIndex == 0)
            {
              endIndex = startIndex + 10;
            }
            else if (endIndex == -1)
            {
                endIndex = classes.Count;
            }

            /* Build OrgMap for Organization name */
            List<Org> orgs = await _context.Orgs.ToListAsync();
            Dictionary<Guid, string> orgMap = [];

            foreach (Org o in orgs)
            {
                orgMap.Add(o.SourcedId, o.Name);
            }

            /* Convert to Class List */
            List<ClassListItem> classList = classes.GetRange(startIndex, Math.Min(endIndex, classes.Count) - startIndex)
                .Select((c) => new ClassListItem {
                    SourcedId = c.SourcedId,
                    Identifier = c.Identifier,
                    Status = c.Status,
                    DateLastModified = c.DateLastModified,
                    Title = c.Title,
                    ClassType = c.ClassType,
                    ClassCode = c.ClassCode,
                    Location = c.Location,
                    OrgSourcedId = c.OrgSourcedId,
                    Organization = orgMap[c.OrgSourcedId]
                }).ToList();
            
            foreach (ClassListItem c in classList)
            {
                c.EnrollmentCount = await GetEnrollmentCount(c);
            }

            return new ClassList {
              Classes = classList,
              Total = classes.Count
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

            /* Match class to enrollments and organization */
            var classEnrollments = await GetClassEnrollments(cls);

            Org? classOrganization = await _context.Orgs.FirstOrDefaultAsync((o) => o.SourcedId == cls.OrgSourcedId);

            return new ClassShow {
                SourcedId = cls.SourcedId,
                Identifier = cls.Identifier,
                Status = cls.Status,
                DateLastModified = cls.DateLastModified,
                Title = cls.Title,
                ClassType = cls.ClassType,
                ClassCode = cls.ClassCode,
                Location = cls.Location,
                OrgSourcedId = cls.OrgSourcedId,
                Enrollments = classEnrollments.ToList(),
                Organization = classOrganization?.Name,
            };
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
