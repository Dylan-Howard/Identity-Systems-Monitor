using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace prognosis_backend.models;

[Table("class")]
public class Class
{
    [Key]
    [JsonPropertyName("sourcedId")]
    [Column("sourced_id")]
    public Guid SourcedId { get; set; }
    [JsonPropertyName("identifier")]
    [Column("identifier")]
    [MaxLength(32)]
    public required string Identifier { get; set; }
    [JsonPropertyName("status")]
    [Column("status")]
    public bool Status { get; set; }
    [JsonPropertyName("dateLastModified")]
    [Column("date_last_modified")]
    public required DateTime DateLastModified { get; set; }
    [JsonPropertyName("title")]
    [Column("title")]
    [MaxLength(48)]
    public required string Title { get; set; }
    [JsonPropertyName("classType")]
    [Column("class_type")]
    [MaxLength(16)]
    public required string ClassType { get; set; }
    [JsonPropertyName("classCode")]
    [Column("class_code")]
    [MaxLength(48)]
    public required string ClassCode { get; set; }
    [JsonPropertyName("location")]
    [Column("location")]
    [MaxLength(16)]
    public required string Location { get; set; }
    [JsonPropertyName("school")]
    [Column("org_sourced_id")]
    public Guid OrgSourcedId { get; set; }

    public static implicit operator string?(Class? v)
    {
        if (v == null) {
            return null;
        }

        string objString = $"{{\n" +
            $"\tIdentifier: {v.Identifier},\n" +
            $"\tStatus: {v.Status},\n" +
            $"\tDateLastModified: {v.DateLastModified},\n" +
            $"\tName: {v.Title},\n" +
            $"\tClassType: {v.ClassType},\n" +
            $"\tClassCode: {v.ClassCode},\n" +
            $"\tLocation: {v.Location},\n" +
            $"\tOrgSourcedId: {v.OrgSourcedId},\n" +
            "}}";

        return objString;
    }
}

