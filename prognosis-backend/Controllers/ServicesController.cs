using Microsoft.Data.SqlClient;
using prognosis_backend.models;

namespace prognosis_backend;

public static class ServicesController
{
    public static async Task<Service?> FetchServiceByName(PrognosisConnectionSettings settings, string serviceName, int retryCount)
        {
            if (retryCount == 0) {
              return null;
            }

            try
            {
              var db = new PrognosisContext(settings);

              Service targetService = db.Services.Single((srv) => srv.Name == serviceName);

              return targetService;
            }
            catch (SqlException e)
            {
                if (e.Number == -2) {
                    return await FetchServiceByName(settings, serviceName, retryCount - 1);
                }

                Console.WriteLine(e.ToString());
                return null;
            }
        }
}