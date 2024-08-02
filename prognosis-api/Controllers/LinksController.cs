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
public class LinksController : ControllerBase
{
    private readonly PrognosisContext _context;

    public LinksController(PrognosisContext context)
    {
        _context = context;
    }

    // GET: api/Links
    [HttpGet]
    public async Task<ActionResult<LinkList>> GetLink(Guid? serviceId)
    {
        List<Link> links = await _context.Links.ToListAsync();

        if (serviceId != null && serviceId != Guid.Empty)
        {
            links.FindAll((l) => l.ServiceId == serviceId);
        }

        return new LinkList {
            Links = links,
            Total = links.Count,
        };
    }

    // GET: api/Links/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Link>> GetLink(Guid id)
    {
        var agent = await _context.Links.FindAsync(id);

        if (agent == null)
        {
            return NotFound();
        }

        return agent;
    }

    // PUT: api/Links/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutLink(Guid id, Link agent)
    {
        if (id != agent.LinkId)
        {
            return BadRequest();
        }

        _context.Entry(agent).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!LinkExists(id))
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

    // POST: api/Links
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Link>> PostLink(Link agent)
    {
        _context.Links.Add(agent);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(PostLink), new { id = Guid.NewGuid() }, agent);
    }

    // DELETE: api/Links/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLink(Guid id)
    {
        var agent = await _context.Links.FindAsync(id);
        if (agent == null)
        {
            return NotFound();
        }

        _context.Links.Remove(agent);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool LinkExists(Guid id)
    {
        return _context.Links.Any(e => e.LinkId == id);
    }
}
