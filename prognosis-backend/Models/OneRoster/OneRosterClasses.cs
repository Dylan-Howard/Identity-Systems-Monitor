using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace prognosis_backend.models
{
    public class OneRosterClass
    {
        [Key]
        [JsonPropertyName("sourcedId")]
        public required string SourcedId { get; set; }
        [JsonPropertyName("status")]
        public string? Status { get; set; }
        [JsonPropertyName("dateLastModified")]
        public DateTime DateLastModified { get; set; }
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("classType")]
        public string? ClassType { get; set; }
        [JsonPropertyName("classCode")]
        public string? ClassCode { get; set; }
        [JsonPropertyName("location")]
        public string? Location { get; set; }
        [JsonPropertyName("school")]
        public OneRosterClassSchool? School { get; set; }
        [JsonPropertyName("periods")]
        public List<string>? Periods { get; set; }

        public static implicit operator string?(OneRosterClass? v)
        {
          if (v == null) {
            return null;
          }

          string orgSourcedId = "";
          if (v.School != null)
          {
              orgSourcedId = v.School.SourcedId.ToString();
          }

          string objString = $"{{\n" +
            $"\tSourcedId: {v.SourcedId},\n" +
            $"\tStatus: {v.Status},\n" +
            $"\tDateLastModified: {v.DateLastModified},\n" +
            $"\tName: {v.Title},\n" +
            $"\tClassType: {v.ClassType},\n" +
            $"\tClassCode: {v.ClassCode},\n" +
            $"\tLocation: {v.Location},\n" +
            $"\tOrgSourcedId: {orgSourcedId},\n" +
          "}}";

          return objString;
        }
        public static implicit operator Class?(OneRosterClass? v)
        {
            if (v == null) {
                return null;
            }

            return new Class {
                Identifier = v.SourcedId,
                Status = v.Status == "active",
                DateLastModified = v.DateLastModified,
                Title = v.Title ?? "",
                ClassType = v.ClassType ?? "",
                ClassCode = v.ClassCode ?? "",
                Location = v.Location ?? "",
                OrgSourcedId = v.School != null ? v.School.SourcedId : Guid.Empty
            };
        }
    }

    public class OneRosterClassSchool {
        [JsonPropertyName("sourcedId")]
        public Guid SourcedId { get; set; }
        [JsonPropertyName("href")]
        public required string Href { get; set; }
        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
    
    public class OneRosterClassesResponse {
        [JsonPropertyName("classes")]
        public required List<OneRosterClass> Classes { get; set; }
    }
}

