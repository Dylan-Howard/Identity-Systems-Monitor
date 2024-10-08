using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prognosis_backend.models;

[Table("job")]
public class Job
{
    [Key]
    [Column("job_id")]
    public Guid JobId { get; set; }
    [Column("service_id")]
    public Guid ServiceId { get; set; }
    [Column("start_date")]
    public DateTime StartDate { get; set; }
    [Column("next_runtime")]
    public DateTime NextRuntime { get; set; }
    [Column("frequency")]
    public required string Frequency { get; set; }
    [Column("active")]
    public bool Active { get; set; }

    public static implicit operator string?(Job? v)
    {
        if (v == null) {
            return null;
        }

        string objString = $"{{\n" +
            $"\tJobId: {v.JobId},\n" +
            $"\tServiceId: {v.ServiceId},\n" +
            $"\tStartDate: {v.StartDate},\n" +
            $"\tNextRuntime: {v.NextRuntime},\n" +
            $"\tFrequency: {v.Frequency},\n" +
            $"\tActive: {v.Active},\n" +
            "}}";

        return objString;
    }
}
