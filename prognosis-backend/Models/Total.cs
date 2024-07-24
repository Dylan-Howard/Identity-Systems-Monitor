using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prognosis_backend.models;

[Table("total")]
public class Total
{
    [Key]
    [Column("total_id")]
    public Guid TotalId { get; set; }
    [ForeignKey("service")]
    [Column("service_id")]
    public Guid ServiceId { get; set; }
    [Column("count")]
    public int Count { get; set; }
    [Column("timestamp")]
    public DateTime Timestamp { get; set; }

    public static implicit operator string?(Total? t)
    {
      if (t == null) {
        return null;
      }

      string objString = $"{t.TotalId}: {{\n" +
        $"\tServiceId: {t.ServiceId},\n" +
        $"\tCount: {t.Count},\n" +
        $"\tTimestamp: {t.Timestamp.ToString()},\n" +
      "}}";

      return objString;
    }
}
