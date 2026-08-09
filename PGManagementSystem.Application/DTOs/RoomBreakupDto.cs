using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Application.DTOs
{
    public class RoomBreakupDto
    {
        public Guid Id { get; set; }
        public string ZoneName { get; set; } = string.Empty;
        public int Type { get; set; } // 1: Non AC, 2: AC
        public int Capacity { get; set; }
        public int OccupiedBeds { get; set; }
        public int VacantBeds { get; set; }
        public decimal RoomRent { get; set; }
        public List<BedBreakupDto> Beds { get; set; } = new List<BedBreakupDto>();
    }
}
