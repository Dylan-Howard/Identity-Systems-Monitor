using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prognosis.Models;

[Table("total")]
public class Total
{
    [Key]
    [Column("total_id")]
    public Guid TotalId { get; set; }
    [Column("service_id")]
    public Guid ServiceId { get; set; }
    [Column("count")]
    public int Count { get; set; }
    [Column("timestamp")]
    public DateTime Timestamp { get; set; }
}
