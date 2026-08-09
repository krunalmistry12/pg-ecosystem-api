using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGManagementSystem.Domain.Enums;

namespace PGManagementSystem.Application.DTOs
{
    public class ZoneDetailDto
    {
        public Guid Id { get; set; } // Ye ZoneId hai
        public string ZoneName { get; set; } = string.Empty;
        public enumZoneType Type { get; set; }
        public int Capacity { get; set; }
        public decimal? RoomRent { get; set; }

        public List<BedDetailDto> Beds { get; set; } = new List<BedDetailDto>();
    }
}
