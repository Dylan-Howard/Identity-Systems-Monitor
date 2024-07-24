using Microsoft.EntityFrameworkCore;
using Prognosis.Models;

namespace Prognosis.Models;

public class PrognosisContext : DbContext
{
    public PrognosisContext(DbContextOptions<PrognosisContext> options)
        : base(options)
    {
        Database.SetCommandTimeout(60);
    }

    public DbSet<Service> Services { get; set; } = default!;

    public DbSet<Profile> Profiles { get; set; } = default!;

    public DbSet<Agent> Agents { get; set; } = default!;
    public DbSet<Total> Totals { get; set; } = default!;
    public DbSet<Link> Links { get; set; } = default!;
}
