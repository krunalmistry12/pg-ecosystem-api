using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGManagementSystem.Application.DTOs;

namespace PGManagementSystem.Application.Interfaces
{
    public interface IFlatService
    {
        Task<FlatDetailDto?> GetFlatByIdAsync(Guid id);

        // 2. Dashboard Cards List
        Task<IEnumerable<FlatSummaryDto>> GetFlatCardsByUserIdAsync(Guid userId);

        // 3. Create Flat
        Task CreateFlatAsync(CreateFlatDto dto);

        // 4. Update Flat
        Task<bool> UpdateFlatAsync(Guid id, CreateFlatDto dto);

        // 5. Delete Flat
        Task<(bool Success, string Message)> DeleteFlatAsync(Guid id);
    }
}
