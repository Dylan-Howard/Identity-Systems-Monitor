using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Prognosis.Models;

[Table("class")]
public class Class
{
    [Key]
    [JsonPropertyName("id")]
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
public class ClassEnrollment
{
    [JsonPropertyName("userSourcedId")]
    public Guid UserSourcedId { get; set; }
    [JsonPropertyName("username")]
    public required string Username { get; set; }
    [JsonPropertyName("role")]
    public required string Role { get; set; }
    [JsonPropertyName("primary")]
    public bool Primary { get; set; }
    [JsonPropertyName("beginDate")]
    public DateOnly? BeginDate { get; set; }
    [JsonPropertyName("endDate")]
    public DateOnly? EndDate { get; set; }
}
public class ClassShow : Class
{
    [JsonPropertyName("enrollments")]
    public required List<ClassEnrollment> Enrollments { get; set; }
    [JsonPropertyName("organization")]
    public string? Organization { get; set; }
}

public class ProfileClass : Class
{
    [JsonPropertyName("organization")]
    public string? Organization { get; set; }
}
public class ClassListItem : Class
{

    [JsonPropertyName("enrollmentCount")]
    public int EnrollmentCount { get; set;}
    [JsonPropertyName("organization")]
    public required string Organization { get; set; }
}
public class ClassList
{
    [Key]
    [JsonPropertyName("data")]
    public required List<ClassListItem> Classes { get; set; }
    [JsonPropertyName("total")]
    public int Total { get; set; }
}