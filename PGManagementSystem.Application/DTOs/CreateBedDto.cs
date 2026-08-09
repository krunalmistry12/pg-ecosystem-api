using System;
using System.Text.Json.Serialization;
using PGManagementSystem.Domain.Enums;

namespace PGManagementSystem.Application.DTOs
{
    public class CreateBedDto
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("bedId")]
        public string? BedId { get; set; }

        [JsonIgnore]
        public Guid? RealBedId
        {
            get
            {
                if (Guid.TryParse(BedId, out var parsedBedId) && parsedBedId != Guid.Empty)
                    return parsedBedId;

                if (Guid.TryParse(Id, out var parsedId) && parsedId != Guid.Empty)
                    return parsedId;

                return null;
            }
        }

        [JsonPropertyName("bedNumber")]
        public string BedNumber { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public enumBedStatus Status { get; set; }

        [JsonPropertyName("tenantName")]
        public string? TenantName { get; set; }

        [JsonPropertyName("bedRent")]
        public decimal? BedRent { get; set; }

        [JsonPropertyName("rent")]
        public decimal? Rent { get; set; }

        [JsonIgnore]
        public decimal RealRent => BedRent ?? Rent ?? 0;
    }
}