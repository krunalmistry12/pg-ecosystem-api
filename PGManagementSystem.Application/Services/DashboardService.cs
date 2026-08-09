using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGManagementSystem.Application.DTOs.Dashboard;
using PGManagementSystem.Application.Interfaces;

namespace PGManagementSystem.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<DashboardResponseDto> GetDashboardSummaryAsync(Guid userId, string role, int? month, int? year)
        {
            return await _dashboardRepository.GetDashboardDataAsync(userId, role, month, year);
        }
    }
}