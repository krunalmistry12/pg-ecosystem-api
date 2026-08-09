using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PGManagementSystem.Application.DTOs;
using PGManagementSystem.Domain.Entities;
using PGManagementSystem.Domain.Enums;

namespace PGManagementSystem.Application.Interfaces
{
    public interface ITenantService
    {
        Task<TenantResponseDto> AddTenantAsync(CreateTenantDto dto);
        Task<TenantResponseDto?> UpdateTenantAsync(long id, UpdateTenantDto dto);
        Task<TenantResponseDto?> GetTenantByIdAsync(long id);
        Task<bool> ChangeTenantStatusAsync(long id, enumTenantStatus status);
        Task<List<TenantResponseDto>> GetTenantsByUserIdAsync(string userId);
        Task<List<TenantResponseDto>> GetTenantsByFlatIdAsync(Guid flatId);
    }
}