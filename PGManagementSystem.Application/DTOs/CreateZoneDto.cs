using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PGManagementSystem.Application.DTOs
{
    public class CreateZoneDto
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("zoneId")]
        public string? ZoneId { get; set; }

        [JsonIgnore]
        public Guid? RealZoneId
        {
            get
            {
                if (Guid.TryParse(ZoneId, out var parsedZoneId) && parsedZoneId != Guid.Empty)
                    return parsedZoneId;

                if (Guid.TryParse(Id, out var parsedId) && parsedId != Guid.Empty)
                    return parsedId;

                return null;
            }
        }

        [JsonPropertyName("zoneName")]
        public string ZoneName { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonPropertyName("capacity")]
        public int Capacity { get; set; }

        [JsonPropertyName("roomRent")]
        public decimal? RoomRent { get; set; }

        [JsonPropertyName("rent")]
        public decimal? Rent { get; set; }

        [JsonIgnore]
        public decimal RealRent => RoomRent ?? Rent ?? 0;

        [JsonPropertyName("beds")]
        public List<CreateBedDto> Beds { get; set; } = new();
    }
}