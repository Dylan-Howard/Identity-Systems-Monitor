using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace prognosis_backend.models
{
    public class OneRosterEnrollment
    {
        [Key]
        [JsonPropertyName("sourcedId")]
        public string? SourcedId { get; set; }
        [JsonPropertyName("status")]
        public string? Status { get; set; }
        [JsonPropertyName("dateLastModified")]
        public DateTime DateLastModified { get; set; }
        [JsonPropertyName("role")]
        public string? Role { get; set; }
        [JsonPropertyName("primary")]
        public bool Primary { get; set; }
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        [JsonPropertyName("beginDate")]
        public DateOnly BeginDate { get; set; }
        [JsonPropertyName("endDate")]
        public DateOnly EndDate { get; set; }
        [JsonPropertyName("user")]
        public OneRosterEnrollmentUser? User { get; set; }
        [Column("user_sourced_id")]
        public string? UserSourcedId { get; set; }
        [JsonPropertyName("class")]
        public OneRosterEnrollmentClass? Class { get; set; }
        [Column("class_sourced_id")]
        public string? ClassSourcedId { get; set; }
        [JsonPropertyName("school")]
        public OneRosterEnrollmentSchool? School { get; set; }
        
        [Column("org_sourced_id")]
        public string? SchoolSourcedId { get; set; }

        public static implicit operator string?(OneRosterEnrollment? v)
        {
          if (v == null) {
            return null;
          }

          string objString = $"{{\n" +
            $"\tSourcedId: {v.SourcedId},\n" +
            $"\tStatus: {v.Status},\n" +
            $"\tDateLastModified: {v.DateLastModified},\n" +
            $"\tBeginDate: {v.BeginDate},\n" +
            $"\tEndDate: {v.EndDate},\n" +
            $"\tType: {v.Type},\n" +
          "}}";

          return objString;
        }
    }
    public class OneRosterEnrollmentClass {
        [JsonPropertyName("sourcedId")]
        public string? SourcedId { get; set; }
        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
    public class OneRosterEnrollmentUser {
        [JsonPropertyName("sourcedId")]
        public Guid SourcedId { get; set; }
        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
    public class OneRosterEnrollmentSchool {
        [JsonPropertyName("sourcedId")]
        public Guid SourcedId { get; set; }
        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
    public class OneRosterEnrollmentsResponse {
        [JsonPropertyName("enrollments")]
        public required List<OneRosterEnrollment> Enrollments { get; set; }
    }
}

