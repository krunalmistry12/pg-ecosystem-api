using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Application.DTOs.Dashboard
{
    public class DashboardResponseDto
    {
        public string OwnerName { get; set; } = string.Empty;
        public RevenueOverviewDto RevenueOverview { get; set; } = new();
        public PropertyMetricsDto PropertyMetrics { get; set; } = new();
        public List<DashboardAlertDto> Alerts { get; set; } = new();
        public List<RecentActivityDto> RecentActivities { get; set; } = new();
    }
}
