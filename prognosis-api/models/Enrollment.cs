using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Prognosis.Models;

[Table("enrollment")]
public class Enrollment
{
    [Key]
    [JsonPropertyName("id")]
    [Column("sourced_id")]
    public Guid SourcedId { get; set; }
    [JsonPropertyName("identifier")]
    [Column("identifier")]
    [MaxLength(16)]
    public required string Identifier { get; set; }
    [JsonPropertyName("status")]
    [Column("status")]
    public bool Status { get; set; }
    [JsonPropertyName("dateLastModified")]
    [Column("date_last_modified")]
    public required DateTime DateLastModified { get; set; }
    [JsonPropertyName("role")]
    [Column("role")]
    [MaxLength(8)]
    public required string Role { get; set; }
    [JsonPropertyName("primary")]
    [Column("primary")]
    public bool Primary { get; set; }
    [JsonPropertyName("beginDate")]
    [Column("begin_date")]
    public DateOnly? BeginDate { get; set; }
    [JsonPropertyName("endDate")]
    [Column("end_date")]
    public DateOnly? EndDate { get; set; }
    [JsonPropertyName("userSourcedId")]
    [Column("user_sourced_id")]
    public Guid UserSourcedId { get; set; }
    [JsonPropertyName("classSourcedId")]
    [Column("class_sourced_id")]
    public Guid ClassSourcedId { get; set; }

    public static implicit operator string?(Enrollment? v)
    {
        if (v == null) {
            return null;
        }

        string objString = $"{{\n" +
            $"\tIdentifier: {v.Identifier},\n" +
            $"\tStatus: {v.Status},\n" +
            $"\tDateLastModified: {v.DateLastModified},\n" +
            $"\tRole: {v.Role},\n" +
            $"\tPrimary: {v.Primary},\n" +
            $"\tUserSourcedId: {v.UserSourcedId},\n" +
            $"\tClassSourcedId: {v.ClassSourcedId},\n" +
            "}}";

        return objString;
    }
}

