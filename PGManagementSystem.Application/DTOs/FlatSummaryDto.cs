using System;
using System.Collections.Generic;

namespace PGManagementSystem.Application.DTOs
{
    public class FlatSummaryDto
    {
        public Guid Id { get; set; }
        public string FlatNumber { get; set; } = string.Empty;
        public string ApartmentName { get; set; } = string.Empty;
        public string PricingType { get; set; } = "BED_WISE";

        // Dynamic Counts & Totals (Quick Stats)
        public int TotalRooms { get; set; }
        public int TotalBeds { get; set; }
        public int OccupiedBeds { get; set; }
        public int VacantBeds { get; set; }
        public decimal TotalFlatExpectedRent { get; set; }

        // Accordion / Expanded Room List UI ke liye
        public List<RoomBreakupDto> RoomBreakup { get; set; } = new List<RoomBreakupDto>();
    }
}