using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace prognosis_backend.models
{
    public class OneRosterOrg
    {
        [Key]
        [JsonPropertyName("sourcedId")]
        public Guid SourcedId { get; set; }
        [JsonPropertyName("status")]
        public required string Status { get; set; }
        [JsonPropertyName("dateLastModified")]
        public DateTime DateLastModified { get; set; }
        [JsonPropertyName("metadata")]
        public OneRosterOrgMetadata? Metadata { get; set; }
        [JsonPropertyName("name")]
        public required string Name { get; set; }
        [JsonPropertyName("identifier")]
        public required string Identifier { get; set; }
        [JsonPropertyName("type")]
        public string? Type { get; set; }

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

        public static implicit operator Org?(OneRosterOrg? v)
        {
            if (v == null) {
                return null;
            }

            return new Org {
                SourcedId = v.SourcedId,
                Status = v.Status == "active",
                DateLastModified = v.DateLastModified,
                Name = v.Name,
                Identifier = v.Identifier,
                Type = v.Type,
                Address = v.Metadata?.Address?.FirstLine,
                City = v.Metadata?.Address?.City,
                State = v.Metadata?.Address?.State,
                Zip = v.Metadata?.Address?.ZipCode,
            };
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

