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
    public class ProfilesController : ControllerBase
    {
        private readonly PrognosisContext _context;

        public ProfilesController(PrognosisContext context)
        {
            _context = context;
        }

        // GET: api/Profiles
        [HttpGet]
        public async Task<ActionResult<ProfileList>> GetProfile(int startIndex, int endIndex, string sort, string order, string? q, bool? status, bool? locked, bool? claimed, string? mfaMethod)
        {
            List<Profile> profiles = await _context.Profiles.ToListAsync();
            
            /* Handle filter*/
            Console.WriteLine("Filtering users");
            Console.WriteLine($"Status: {status}");
            if (q != null)
            {
                profiles = profiles.FindAll(
                    (p) => String.Compare(
                        p.Email.Substring(0, Math.Min(q.Length, p.Email.Length)).ToLower(),
                        q.ToLower()
                    ) == 0
                );
            }
            else if (status != null)
            {
                Console.WriteLine("Finding users by status");
                profiles = profiles.FindAll((p) => p.Status == status);
            }
            else if (locked != null)
            {
                profiles = profiles.FindAll((p) => p.Locked == locked);
            }
            else if (claimed != null)
            {
                profiles = profiles.FindAll((p) => p.Claimed == claimed);
            }
            else if (mfaMethod != null)
            {
                profiles = profiles.FindAll(
                    (p) => String.Compare(p.MfaMethod.ToLower(), mfaMethod.ToLower()) == 0
                );
            }
            

            /* Handle Sorting */
            switch (sort)
            {
                case "id":
                    if (order == "DESC")
                    {
                        profiles = profiles.OrderByDescending((p) => p.Identifier).ToList();
                    }
                    else
                    {
                        profiles = profiles.OrderBy((p) => p.Identifier).ToList();
                    }
                    break;
                case "firstName":
                    if (order == "DESC")
                    {
                        profiles = profiles.OrderByDescending((p) => p.FirstName).ToList();
                    }
                    else
                    {
                        profiles = profiles.OrderBy((p) => p.FirstName).ToList();
                    }
                    break;
                case "lastName":
                    if (order == "DESC")
                    {
                        profiles = profiles.OrderByDescending((p) => p.LastName).ToList();
                    }
                    else
                    {
                        profiles = profiles.OrderBy((p) => p.LastName).ToList();
                    }
                    break;
                case "email":
                    if (order == "DESC")
                    {
                        profiles = profiles.OrderByDescending((p) => p.Email).ToList();
                    }
                    else
                    {
                        profiles = profiles.OrderBy((p) => p.Email).ToList();
                    }
                    break;
                case "birthdate":
                    if (order == "DESC")
                    {
                        profiles = profiles.OrderByDescending((p) => p.Birthdate).ToList();
                    }
                    else
                    {
                        profiles = profiles.OrderBy((p) => p.Birthdate).ToList();
                    }
                    break;
                case "status":
                    if (order == "DESC")
                    {
                        profiles = profiles.OrderByDescending((p) => p.Status).ToList();
                    }
                    else
                    {
                        profiles = profiles.OrderBy((p) => p.Status).ToList();
                    }
                    break;
                ;
                case "claimed":
                    if (order == "DESC")
                    {
                        profiles = profiles.OrderByDescending((p) => p.Claimed).ToList();
                    }
                    else
                    {
                        profiles = profiles.OrderBy((p) => p.Claimed).ToList();
                    }
                    break;
                ;
                case "locked":
                    if (order == "DESC")
                    {
                        profiles = profiles.OrderByDescending((p) => p.Locked).ToList();
                    }
                    else
                    {
                        profiles = profiles.OrderBy((p) => p.Locked).ToList();
                    }
                    break;
                ;
                case "mfaMethod":
                    if (order == "DESC")
                    {
                        profiles = profiles.OrderByDescending((p) => p.MfaMethod).ToList();
                    }
                    else
                    {
                        profiles = profiles.OrderBy((p) => p.MfaMethod).ToList();
                    }
                    break;
                ;
            }

            /* Handle array slicing */
            if (endIndex == 0)
            {
              endIndex = 10;
            }

            return new ProfileList {
              Profiles = profiles.GetRange(startIndex, Math.Min(endIndex, profiles.Count) - startIndex),
              Total = profiles.Count
            };
        }

        // GET: api/Profiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProfileShow>> GetProfile(Guid id)
        {
            var profile = await _context.Profiles.FindAsync(id);

            if (profile == null)
            {
                return NotFound();
            }

            /* Add Linked Accounts to Profile */
            List<Service> services = await _context.Services.ToListAsync();
            Dictionary<Guid, string> serviceMap = [];

            foreach (Service s in services)
            {
                serviceMap.Add(s.ServiceId, s.Name);
            }

            List<Link> links = await _context.Links.ToListAsync();
            links = links.FindAll((l) => l.ProfileId == profile.ProfileId);

            List<ProfileLink> profileLinks = [];

            Console.WriteLine("Finding linked accounts");

            foreach (Link l in links)
            {
                string? serviceName = serviceMap[l.ServiceId];
                if (serviceName == null)
                {
                    continue;
                }

                profileLinks.Add(new ProfileLink {
                    ServiceName = serviceName,
                    LinkedAccount = l,
                });
            }

            /* Add Classes to Profile */
            List<ProfileClass> profileClasses = [];

            Service? oneRosterService = await _context.Services
                .FirstOrDefaultAsync((s) => s.Name == "One Roster");
            Link? oneRosterlink = links.FirstOrDefault(
                (l) => l.ServiceId == oneRosterService?.ServiceId);

            if (oneRosterlink != null)
            {
                List<Enrollment> enrollments = await _context.Enrollments
                    .Where((e) => e.UserSourcedId == oneRosterlink.LinkId)
                    .ToListAsync();

                Console.WriteLine(enrollments.Count);

                List<Class> classes = await _context.Classes.ToListAsync();

                classes = classes.FindAll((c) => enrollments.Find((e) => e.ClassSourcedId == c.SourcedId) != null);

                List<Org> orgs = await _context.Orgs.ToListAsync();

                foreach (Class c in classes)
                {
                    profileClasses.Add(new ProfileClass{
                        SourcedId = c.SourcedId,
                        Identifier = c.Identifier,
                        Status = c.Status,
                        DateLastModified = c.DateLastModified,
                        Title = c.Title,
                        ClassType = c.ClassType,
                        ClassCode = c.ClassCode,
                        Location = c.Location,
                        OrgSourcedId = c.OrgSourcedId,
                        Organization = orgs.FirstOrDefault((o) => o.SourcedId == c.OrgSourcedId)?.Name
                    });
                }
            }

            return new ProfileShow {
              ProfileId = profile.ProfileId,
              Identifier = profile.Identifier,
              FirstName = profile.FirstName,
              MiddleName = profile.MiddleName,
              LastName = profile.LastName,
              Email = profile.Email,
              Birthdate = profile.Birthdate,
              Status = profile.Status,
              Claimed = profile.Claimed,
              Locked = profile.Locked,
              MfaMethod = profile.MfaMethod,
              Links = profileLinks,
              Classes = profileClasses,
            };
        }

        // PUT: api/Profiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfile(Guid id, Profile profile)
        {
            if (id != profile.ProfileId)
            {
                return BadRequest();
            }

            _context.Entry(profile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileExists(id))
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

        // POST: api/Profiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Profile>> PostProfile(Profile profile)
        {
            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostProfile), new { id = Guid.NewGuid() }, profile);
        }

        // DELETE: api/Profiles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfile(Guid id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }

            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProfileExists(Guid id)
        {
            return _context.Profiles.Any(e => e.ProfileId == id);
        }
  }
}
