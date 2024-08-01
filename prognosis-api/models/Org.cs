using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Prognosis.Models;
    [Table("org")]
    public class Org
    {
        [Key]
        [JsonPropertyName("id")]
        [Column("sourced_id")]
        public Guid SourcedId { get; set; }
        [JsonPropertyName("status")]
        [Column("status")]
        public bool Status { get; set; }
        [JsonPropertyName("dateLastModified")]
        [Column("date_last_modified")]
        public DateTime DateLastModified { get; set; }
        [JsonPropertyName("name")]
        [Column("name")]
        [MaxLength(64)]
        public required string Name { get; set; }
        [JsonPropertyName("identifier")]
        [Column("identifier")]
        [MaxLength(36)]
        public required string Identifier { get; set; }
        [JsonPropertyName("type")]
        [Column("type")]
        [MaxLength(8)]
        public required string Type { get; set; }
        [JsonPropertyName("address")]
        [Column("address")]
        [MaxLength(32)]
        public required string Address { get; set; }
        [JsonPropertyName("city")]
        [Column("city")]
        [MaxLength(32)]
        public required string City { get; set; }
        [JsonPropertyName("state")]
        [Column("state")]
        [MaxLength(16)]
        public string? State { get; set; }
        [JsonPropertyName("zip")]
        [Column("zip")]
        [MaxLength(10)]
        public string? Zip { get; set; }

        public static implicit operator string?(Org? v)
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
                $"\tAddress: {v.Address},\n" +
                $"\tCity: {v.City},\n" +
                $"\tState: {v.State},\n" +
                $"\tZip: {v.Zip},\n" +
                "}}";

            return objString;
        }
    }

public class OrgList
{
    [Key]
    [JsonPropertyName("data")]
    public required List<Org> Orgs { get; set; }
    [JsonPropertyName("total")]
    public int Total { get; set; }
}