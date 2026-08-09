using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Application.DTOs.Dashboard
{
    public class RevenueOverviewDto
    {
        public string MonthName { get; set; } = "August";
        public decimal TotalExpectedRevenue { get; set; }
        public decimal TotalCollected { get; set; }
        public decimal TotalPendingDue { get; set; }
    }
}
