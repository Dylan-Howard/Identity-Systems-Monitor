using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using prognosis_backend.models;

public class PrognosisContext : DbContext
{
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Total> Totals { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Link> Links { get; set; }
    public DbSet<Org> Orgs { get; set; }
    // public DbSet<OneRosterClass> Classes { get; set; }
    // public DbSet<OneRosterEnrollment> Enrollments { get; set; }

    public string connString { get; }

    public PrognosisContext(PrognosisConnectionSettings settings)
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

        builder.DataSource = settings.Url;
        builder.UserID = settings.Username;
        builder.Password = settings.Password;
        builder.InitialCatalog = "master";
        builder.TrustServerCertificate = true;
         
        connString = builder.ConnectionString;
    }
    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer(connString);
}

public class PrognosisConnectionSettings
{
    public required string Url { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}