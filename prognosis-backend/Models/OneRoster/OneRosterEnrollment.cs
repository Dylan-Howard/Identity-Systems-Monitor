using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace prognosis_backend.models
{
    public class OneRosterEnrollment
    {
        [Key]
        [JsonPropertyName("sourcedId")]
        public required string SourcedId { get; set; }
        [JsonPropertyName("status")]
        public required string Status { get; set; }
        [JsonPropertyName("dateLastModified")]
        public DateTime DateLastModified { get; set; }
        [JsonPropertyName("role")]
        public string? Role { get; set; }
        [JsonPropertyName("primary")]
        public string? Primary { get; set; }
        [JsonPropertyName("beginDate")]
        public DateOnly? BeginDate { get; set; }
        [JsonPropertyName("endDate")]
        public DateOnly? EndDate { get; set; }
        [JsonPropertyName("user")]
        public required OneRosterEnrollmentUser User { get; set; }
        [JsonPropertyName("class")]
        public required OneRosterEnrollmentClass Class { get; set; }
        [JsonPropertyName("school")]
        public OneRosterEnrollmentSchool? School { get; set; }

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
          "}}";

          return objString;
        }
        public static implicit operator Enrollment?(OneRosterEnrollment? v)
        {
            if (v == null) {
                return null;
            }

            return new Enrollment {
                Identifier = v.SourcedId,
                Status = v.Status == "active",
                DateLastModified = v.DateLastModified,
                Role = v.Role ?? "",
                Primary = v.Primary == "true",
                BeginDate = v.BeginDate,
                EndDate = v.EndDate,
                UserSourcedId = v.User.SourcedId,
                ClassSourcedId = Guid.Empty,
            };
        }
    }
    public class OneRosterEnrollmentClass {
        [JsonPropertyName("sourcedId")]
        public required string SourcedId { get; set; }
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

