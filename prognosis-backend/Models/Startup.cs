using Microsoft.Extensions.Configuration;

namespace prognosis_backend.models;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
}