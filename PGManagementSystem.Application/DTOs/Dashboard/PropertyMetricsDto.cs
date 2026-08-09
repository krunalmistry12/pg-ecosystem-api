using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Application.DTOs.Dashboard
{
    public class PropertyMetricsDto
    {
        public int TotalRooms { get; set; }
        public int OccupiedRooms { get; set; }
        public int VacantRooms { get; set; }
        public int ActiveTenants { get; set; }
        public int NewJoinersThisMonth { get; set; }
        public int OccupancyPercentage { get; set; }
    }
}
