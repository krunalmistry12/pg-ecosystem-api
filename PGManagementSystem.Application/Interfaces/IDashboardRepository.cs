using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGManagementSystem.Application.DTOs.Dashboard;

namespace PGManagementSystem.Application.Interfaces
{
    public interface IDashboardRepository
    {
        Task<DashboardResponseDto> GetDashboardDataAsync(Guid userId, string role, int? month, int? year);
    }
}
