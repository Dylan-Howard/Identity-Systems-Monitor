using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Prognosis.Models;

[Table("job")]
public class Job
{
    [Key]
    [Column("job_id")]
    [JsonPropertyName("id")]
    public Guid JobId { get; set; }
    [Column("service_id")]
    [JsonPropertyName("serviceId")]
    public Guid ServiceId { get; set; }
    [Column("start_date")]
    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }
    [Column("next_runtime")]
    [JsonPropertyName("nextRunTime")]
    public DateTime NextRunTime { get; set; }
    [Column("frequency")]
    [JsonPropertyName("frequency")]
    public required string Frequency { get; set; }
    [Column("active")]
    [JsonPropertyName("active")]
    public bool Active { get; set; }
}

public class JobListItem : Job
{
    [Key]
    [JsonPropertyName("serviceName")]
    public required string ServiceName { get; set; }
}
public class JobList
{
    [Key]
    [JsonPropertyName("data")]
    public required List<JobListItem> Jobs { get; set; }
    [JsonPropertyName("total")]
    public int Total { get; set; }
}