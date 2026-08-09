using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Application.DTOs.Dashboard
{
    public class DashboardAlertDto
    {
        public string Id { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }
}
