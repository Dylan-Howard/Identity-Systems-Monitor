using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace prognosis_backend.models
{
    public class OneRosterOrg
    {
        [JsonPropertyName("sourcedId")]
        public Guid SourcedId { get; set; }
        [JsonPropertyName("status")]
        public string? Status { get; set; }
        [JsonPropertyName("dateLastModified")]
        public DateTime DateLastModified { get; set; }
        [JsonPropertyName("metadata")]
        public OneRosterOrgMetadata? Metadata { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("identifier")]
        public string? Identifier { get; set; }
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        [Column("address")]
        public string? Address { get; set; }
        [Column("city")]
        public string? City { get; set; }
        [Column("state")]
        public string? State { get; set; }
        [Column("zip")]
        public string? Zip { get; set; }

        public static implicit operator string?(OneRosterOrg? v)
        {
            if (v == null) {
              return null;
            }

            string objString = $"{{\n" +
              $"\tSourcedId: {v.SourcedId},\n" +
              $"\tStatus: {v.Status},\n" +
              $"\tDateLastModified: {v.DateLastModified},\n" +
              $"\tName: {v.Name},\n" +
              $"\tIdentifier: {v.Identifier},\n" +
              $"\tType: {v.Type},\n" +
            "}}";

            return objString;
        }
    }
    public class OneRosterOrgAddress {
        [JsonPropertyName("city")]
        public string? City { get; set; }
        [JsonPropertyName("firstLine")]
        public string? FirstLine { get; set; }
        [JsonPropertyName("state")]
        public string? State { get; set; }
        [JsonPropertyName("zipcode")]
        public string? ZipCode { get; set; }
    }
    public class OneRosterOrgMetadata {
        [JsonPropertyName("ic_address")]
        public OneRosterOrgAddress? Address { get; set; }
    }
    public class OneRosterOrgsResponse {
        [JsonPropertyName("orgs")]
        public required List<OneRosterOrg> Orgs { get; set; }
    }
}

