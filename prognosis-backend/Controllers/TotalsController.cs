
using Microsoft.Data.SqlClient;
using prognosis_backend.models;

namespace prognosis_backend;

public static class TotalsController
{
    public static async Task<bool> LogSyncResults(PrognosisConnectionSettings settings, Guid serviceId, int total, int retryCount)
        {
            if (retryCount == 0) {
                return false;
            }

            try
            {
                var db = new PrognosisContext(settings);

                Total toAdd = new Total {
                  ServiceId = serviceId,
                  Count = total,
                  Timestamp = DateTime.Now,
                };

                await db.AddAsync(toAdd);
                await db.SaveChangesAsync();
            }
            catch (SqlException e)
            {
                if (e.Number == -2) {
                    return await LogSyncResults(settings, serviceId, total, retryCount - 1);
                }

                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }
}