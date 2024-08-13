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
    public class DashboardController : ControllerBase
    {
        private readonly PrognosisContext _context;

        public DashboardController(PrognosisContext context)
        {
            _context = context;
        }

        // GET: api/Dashboard
        [HttpGet]
        public async Task<ActionResult<Dashboard>> GetDashboard()
        {
            /* Collects totals data */
            FormattableString totalsQuery = $"SELECT tot.[total_id], tot.[service_id], tot.[count], tot.[timestamp] FROM( SELECT subLog.[service_id], MAX(subLog.[timestamp]) AS [timestamp] FROM dbo.[total] subLog GROUP BY subLog.[service_id]) AS [log] INNER JOIN dbo.[total] tot ON tot.[service_id] = [log].[service_id] AND tot.[timestamp] = [log].[timestamp]";
            IList<Total> totals = await _context.Totals.FromSql(totalsQuery).ToListAsync();
            IList<Service> services = await _context.Services.ToListAsync();

            IList<DashboardTotal> dashboardTotals = [];
            foreach (Total t in totals)
            {
                string serviceName = services.First((s) => s.ServiceId == t.ServiceId).Name;
                dashboardTotals.Add(new DashboardTotal {
                    Id = t.ServiceId.ToString(),
                    Title = serviceName,
                    Total = t.Count
                });
            }

            /* Collects trends data */
            IList<DashboardTrend> trends = new List<DashboardTrend>();
            foreach (Service s in services)
            {
                FormattableString trendsQuery = $"SELECT TOP(30) [timestamp] AS [name], [count] FROM dbo.[total] WHERE [service_id] = '{s.ServiceId}'";
                IList<Total> serviceTotals = await _context.Totals.Where((t) => t.ServiceId == s.ServiceId)
                                                                  .OrderBy((t) => t.Timestamp)
                                                                  .ToListAsync();
                IList<DashboardTrendDataPoint> points = [];

                foreach (Total t in serviceTotals)
                {
                    points.Add(new DashboardTrendDataPoint {
                        Name = t.Timestamp.ToString(),
                        Count = t.Count
                    });
                }

              trends.Add(new DashboardTrend {
                  Id = s.ServiceId.ToString(),
                  Title = s.Name,
                  Data = points
              });
            }

            /* Collects trends data */
            IList<DashboardChange> changes = new List<DashboardChange>();
            changes.Add(new DashboardChange {
              User = "Placeholder",
              CurrentId = "2120000000",
              FormerId = "1960000000",
              Timestamp = DateTime.Now
            });

            return new Dashboard {
              Id = "warren",
              Totals = dashboardTotals,
              Trends = trends,
              Changes = changes
            };
        }
    }
}
