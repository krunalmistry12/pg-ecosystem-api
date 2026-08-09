using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PGManagementSystem.Application.DTOs;
using PGManagementSystem.Application.Interfaces;
using PGManagementSystem.Domain.Entities;
using PGManagementSystem.Domain.Enums;

namespace PGManagementSystem.Application.Services
{
    public class TenantService : ITenantService
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IBedRepository _bedRepository;

        public TenantService(ITenantRepository tenantRepository, IBedRepository bedRepository)
        {
            _tenantRepository = tenantRepository;
            _bedRepository = bedRepository;
        }

        // =========================================================
        // 1. FAST CREATE TENANT (Returns DTO for clean API response)
        // =========================================================
        public async Task<TenantResponseDto> AddTenantAsync(CreateTenantDto dto)
        {
            int lockInMonths = dto.LockInPeriodMonths > 0 ? dto.LockInPeriodMonths : 6;
            DateTime agreementEnd = dto.JoiningDate.AddMonths(lockInMonths);

            string? idProofUrl = dto.IdProofFile != null ? await SaveFileAsync(dto.IdProofFile, "id-proofs") : null;
            string? photoUrl = dto.TenantPhotoFile != null ? await SaveFileAsync(dto.TenantPhotoFile, "photos") : null;

            var tenant = new TenantMaster
            {
                Name = dto.Name,
                Phone = dto.Phone,

                // Foreign Keys
                FlatId = dto.FlatId,
                RoomId = dto.AllocationType == enumAllocationType.FULL_FLAT ? null : dto.RoomId,
                BedId = dto.AllocationType == enumAllocationType.BED ? dto.BedId : null,

                AllocationType = dto.AllocationType,
                Rent = dto.Rent,
                JoiningDate = dto.JoiningDate,

                Email = dto.Email,
                EmergencyPhone = dto.EmergencyPhone,
                Deposit = dto.Deposit ?? 0m,
                AdvancePaid = dto.AdvancePaid ?? 0m,
                DueDate = dto.DueDate > 0 ? dto.DueDate : 5,
                PaymentMethod = string.IsNullOrWhiteSpace(dto.PaymentMethod) || dto.PaymentMethod == "string" ? "UPI" : dto.PaymentMethod,
                StartingMeterReading = dto.StartingMeterReading ?? 0.0,
                LockInPeriodMonths = lockInMonths,
                AgreementEndDate = agreementEnd,

                IdProofType = string.IsNullOrWhiteSpace(dto.IdProofType) || dto.IdProofType == "string" ? "ID_PROOF" : dto.IdProofType,
                IdProofNumber = dto.IdProofNumber,
                IdProofUrl = idProofUrl,
                TenantPhotoUrl = photoUrl,

                PoliceVerificationStatus = "NOT_STARTED",
                Status = enumTenantStatus.ACTIVE,
                CreatedAt = DateTime.UtcNow
            };

            await _tenantRepository.AddAsync(tenant);

            // AUTO-OCCUPY BEDS
            await UpdateBedOccupancyOnAllocationAsync(dto.AllocationType, dto.FlatId, dto.RoomId, dto.BedId, dto.Name);

            await _tenantRepository.SaveChangesAsync();

            var createdTenant = await _tenantRepository.GetByIdAsync(tenant.Id);
            return MapToResponseDto(createdTenant ?? tenant);
        }

        // =========================================================
        // HELPER METHOD 1: PURANE BED/ROOM/FLAT KO VACANT KARNA
        // =========================================================
        private async Task ReleaseBedOccupancyAsync(
            enumAllocationType oldAllocationType, Guid oldFlatId, Guid? oldRoomId, Guid? oldBedId)
        {
            if (oldAllocationType == enumAllocationType.FULL_FLAT && oldFlatId != Guid.Empty)
            {
                var bedsInFlat = await _bedRepository.GetBedsByFlatIdAsync(oldFlatId);
                foreach (var bed in bedsInFlat)
                {
                    bed.Status = enumBedStatus.Vacant;
                    bed.TenantName = null;
                    bed.UpdatedAt = DateTime.UtcNow;
                    await _bedRepository.UpdateAsync(bed);
                }
            }
            else if (oldAllocationType == enumAllocationType.ROOM && oldRoomId.HasValue)
            {
                var bedsInZone = await _bedRepository.GetBedsByZoneIdAsync(oldRoomId.Value);
                foreach (var bed in bedsInZone)
                {
                    bed.Status = enumBedStatus.Vacant;
                    bed.TenantName = null;
                    bed.UpdatedAt = DateTime.UtcNow;
                    await _bedRepository.UpdateAsync(bed);
                }
            }
            else if (oldAllocationType == enumAllocationType.BED && oldBedId.HasValue)
            {
                var bed = await _bedRepository.GetByIdAsync(oldBedId.Value);
                if (bed != null)
                {
                    bed.Status = enumBedStatus.Vacant;
                    bed.TenantName = null;
                    bed.UpdatedAt = DateTime.UtcNow;
                    await _bedRepository.UpdateAsync(bed);
                }
            }
        }

        // =========================================================
        // HELPER METHOD 2: NAYE BED/ROOM/FLAT KO OCCUPIED KARNA
        // =========================================================
        private async Task UpdateBedOccupancyOnAllocationAsync(
            enumAllocationType allocationType, Guid flatId, Guid? roomId, Guid? bedId, string tenantName)
        {
            if (allocationType == enumAllocationType.FULL_FLAT)
            {
                var bedsInFlat = await _bedRepository.GetBedsByFlatIdAsync(flatId);
                foreach (var bed in bedsInFlat)
                {
                    bed.Status = enumBedStatus.Occupied;
                    bed.TenantName = $"{tenantName} (Full Flat)";
                    bed.UpdatedAt = DateTime.UtcNow;
                    await _bedRepository.UpdateAsync(bed);
                }
            }
            else if (allocationType == enumAllocationType.ROOM && roomId.HasValue)
            {
                var bedsInZone = await _bedRepository.GetBedsByZoneIdAsync(roomId.Value);
                foreach (var bed in bedsInZone)
                {
                    bed.Status = enumBedStatus.Occupied;
                    bed.TenantName = $"{tenantName} (Full Room)";
                    bed.UpdatedAt = DateTime.UtcNow;
                    await _bedRepository.UpdateAsync(bed);
                }
            }
            else if (allocationType == enumAllocationType.BED && bedId.HasValue)
            {
                var bed = await _bedRepository.GetByIdAsync(bedId.Value);
                if (bed != null)
                {
                    bed.Status = enumBedStatus.Occupied;
                    bed.TenantName = tenantName;
                    bed.UpdatedAt = DateTime.UtcNow;
                    await _bedRepository.UpdateAsync(bed);
                }
            }
        }

        // =========================================================
        // 3. GET TENANT BY ID
        // =========================================================
        public async Task<TenantResponseDto?> GetTenantByIdAsync(long id)
        {
            var tenant = await _tenantRepository.GetByIdAsync(id);
            if (tenant == null) return null;

            return MapToResponseDto(tenant);
        }

        // =========================================================
        // 4. CHANGE TENANT STATUS (FIXED: Clears FlatId too on Activation reset)
        // =========================================================
        public async Task<bool> ChangeTenantStatusAsync(long id, enumTenantStatus status)
        {
            var tenant = await _tenantRepository.GetByIdAsync(id);
            if (tenant == null) return false;

            if (tenant.Status == status) return true;

            tenant.Status = status;

            if (status == enumTenantStatus.INACTIVE)
            {
                tenant.VacatedAt = DateTime.UtcNow;
                await ReleaseBedOccupancyAsync(tenant.AllocationType, tenant.FlatId, tenant.RoomId, tenant.BedId);
            }
            else if (status == enumTenantStatus.ACTIVE)
            {
                tenant.VacatedAt = null;

                // Purane beds release kar dein agar pehle se assigned the
                await ReleaseBedOccupancyAsync(tenant.AllocationType, tenant.FlatId, tenant.RoomId, tenant.BedId);

                // Fresh state ke liye saari location IDs reset karein
                tenant.FlatId = Guid.Empty;
                tenant.RoomId = null;
                tenant.BedId = null;
                tenant.AllocationType = default(enumAllocationType);
            }

            _tenantRepository.Update(tenant);
            return await _tenantRepository.SaveChangesAsync();
        }

        // =========================================================
        // 5. GET ALL TENANTS FOR OWNER DASHBOARD
        // =========================================================
        public async Task<List<TenantResponseDto>> GetTenantsByUserIdAsync(string userId)
        {
            if (!Guid.TryParse(userId, out var userGuid))
            {
                return new List<TenantResponseDto>();
            }

            var tenants = await _tenantRepository.GetTenantsByUserIdAsync(userGuid);
            return tenants.Select(MapToResponseDto).ToList();
        }

        // =========================================================
        // 6. GET TENANTS FOR FLAT DETAILS VIEW
        // =========================================================
        public async Task<List<TenantResponseDto>> GetTenantsByFlatIdAsync(Guid flatId)
        {
            var tenants = await _tenantRepository.GetByFlatIdAsync(flatId);
            return tenants.Select(MapToResponseDto).ToList();
        }

        // =========================================================
        // 7. UPDATE TENANT
        // =========================================================
        public async Task<TenantResponseDto?> UpdateTenantAsync(long id, UpdateTenantDto dto)
        {
            var tenant = await _tenantRepository.GetByIdAsync(id);
            if (tenant == null) return null;

            var oldAllocationType = tenant.AllocationType;
            var oldFlatId = tenant.FlatId;
            var oldRoomId = tenant.RoomId;
            var oldBedId = tenant.BedId;

            bool isLocationChanged = false;

            if (dto.FlatId.HasValue && dto.FlatId.Value != tenant.FlatId && dto.FlatId.Value != Guid.Empty)
            {
                tenant.FlatId = dto.FlatId.Value;
                isLocationChanged = true;
            }

            if (dto.AllocationType.HasValue && dto.AllocationType.Value != tenant.AllocationType)
            {
                tenant.AllocationType = dto.AllocationType.Value;
                isLocationChanged = true;
            }

            if (dto.RoomId.HasValue && dto.RoomId.Value != tenant.RoomId)
            {
                tenant.RoomId = dto.RoomId;
                isLocationChanged = true;
            }

            if (dto.BedId.HasValue && dto.BedId.Value != tenant.BedId)
            {
                tenant.BedId = dto.BedId;
                isLocationChanged = true;
            }

            if (isLocationChanged)
            {
                await ReleaseBedOccupancyAsync(oldAllocationType, oldFlatId, oldRoomId, oldBedId);
                await UpdateBedOccupancyOnAllocationAsync(tenant.AllocationType, tenant.FlatId, tenant.RoomId, tenant.BedId, tenant.Name);
            }

            // Basic Details Updates
            if (!string.IsNullOrWhiteSpace(dto.Name) && dto.Name != "string")
                tenant.Name = dto.Name;
            if (!string.IsNullOrWhiteSpace(dto.Phone) && dto.Phone != "string")
                tenant.Phone = dto.Phone;
            if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != "string")
                tenant.Email = dto.Email;
            if (!string.IsNullOrWhiteSpace(dto.EmergencyPhone) && dto.EmergencyPhone != "string")
                tenant.EmergencyPhone = dto.EmergencyPhone;

            // Financial Updates
            if (dto.Rent.HasValue && dto.Rent.Value > 0)
                tenant.Rent = dto.Rent.Value;
            if (dto.Deposit.HasValue && dto.Deposit.Value >= 0)
                tenant.Deposit = dto.Deposit.Value;
            if (dto.DueDate.HasValue && dto.DueDate.Value > 0)
                tenant.DueDate = dto.DueDate.Value;
            if (!string.IsNullOrWhiteSpace(dto.PaymentMethod) && dto.PaymentMethod != "string")
                tenant.PaymentMethod = dto.PaymentMethod;

            // Police Verification & Document Proofs
            if (!string.IsNullOrWhiteSpace(dto.PoliceVerificationStatus) && dto.PoliceVerificationStatus != "string")
                tenant.PoliceVerificationStatus = dto.PoliceVerificationStatus;
            if (!string.IsNullOrWhiteSpace(dto.IdProofType) && dto.IdProofType != "string")
                tenant.IdProofType = dto.IdProofType;
            if (!string.IsNullOrWhiteSpace(dto.IdProofNumber) && dto.IdProofNumber != "string")
                tenant.IdProofNumber = dto.IdProofNumber;

            // File Uploads
            if (dto.IdProofFile != null)
                tenant.IdProofUrl = await SaveFileAsync(dto.IdProofFile, "id-proofs");

            if (dto.TenantPhotoFile != null)
                tenant.TenantPhotoUrl = await SaveFileAsync(dto.TenantPhotoFile, "photos");

            _tenantRepository.Update(tenant);
            await _tenantRepository.SaveChangesAsync();

            var reloadedTenant = await _tenantRepository.GetByIdAsync(id);
            return reloadedTenant != null ? MapToResponseDto(reloadedTenant) : null;
        }

        // =========================================================
        // HELPER METHOD: CENTRALIZED DTO MAPPER
        // =========================================================
        private static TenantResponseDto MapToResponseDto(TenantMaster t)
        {
            return new TenantResponseDto
            {
                Id = t.Id.ToString(),

                FlatId = t.FlatId.ToString(),
                ApartmentName = t.Flat?.ApartmentName ?? string.Empty,
                FlatNumber = t.Flat?.FlatNumber ?? string.Empty,

                RoomId = t.RoomId?.ToString(),
                RoomName = t.Room?.ZoneName ?? "N/A",

                BedId = t.BedId?.ToString(),
                BedName = t.Bed?.BedNumber ?? "N/A",

                Name = t.Name,
                Phone = t.Phone,
                Email = t.Email,
                EmergencyPhone = t.EmergencyPhone,

                Status = t.Status.ToString().ToUpper(),
                AllocationType = t.AllocationType.ToString().ToUpper(),
                PoliceVerificationStatus = t.PoliceVerificationStatus ?? "NOT_STARTED",

                Rent = t.Rent,
                Deposit = t.Deposit ?? 0m,
                AdvancePaid = t.AdvancePaid ?? 0m,
                DueDate = t.DueDate,
                PaymentMethod = t.PaymentMethod,

                StartingMeterReading = t.StartingMeterReading,
                LockInPeriodMonths = t.LockInPeriodMonths,
                JoiningDate = t.JoiningDate,
                AgreementEndDate = t.AgreementEndDate,
                CreatedAt = t.CreatedAt,

                IdProofType = t.IdProofType,
                IdProofNumber = t.IdProofNumber,
                IdProofUrl = t.IdProofUrl,
                TenantPhotoUrl = t.TenantPhotoUrl
            };
        }

        // =========================================================
        // HELPER METHOD: FILE SAVER
        // =========================================================
        private async Task<string?> SaveFileAsync(IFormFile? file, string subFolder)
        {
            if (file == null || file.Length == 0) return null;

            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", subFolder);

            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{subFolder}/{uniqueFileName}";
        }
    }
}