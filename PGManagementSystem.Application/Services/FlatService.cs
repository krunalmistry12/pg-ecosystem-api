using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PGManagementSystem.Application.DTOs;
using PGManagementSystem.Application.Interfaces;
using PGManagementSystem.Domain.Entities;
using PGManagementSystem.Domain.Enums;

namespace PGManagementSystem.Application.Services
{
    public class FlatService : IFlatService
    {
        private readonly IFlatRepository _flatRepo;
        private readonly IUserRepository _userRepo;
        private readonly ILogger<FlatService> _logger;

        public FlatService(IFlatRepository flatRepo, IUserRepository userRepo, ILogger<FlatService> logger)
        {
            _flatRepo = flatRepo;
            _userRepo = userRepo;
            _logger = logger;
        }

        // 1. Get Single Flat Details (Returns FlatDetailDto for React Native Edit Screen)
        public async Task<FlatDetailDto?> GetFlatByIdAsync(Guid id)
        {
            var flat = await _flatRepo.GetFlatByIdAsync(id);
            if (flat == null) return null;

            return new FlatDetailDto
            {
                FlatId = flat.FlatId,
                FlatNumber = flat.FlatNumber,
                ApartmentName = flat.ApartmentName,
                PricingType = flat.PricingType,
                Zones = flat.Zones.Select(z => new ZoneDetailDto
                {
                    Id = z.ZoneId,
                    ZoneName = z.ZoneName,
                    Type = z.Type,
                    Capacity = z.Capacity,
                    RoomRent = z.RoomRent,
                    Beds = z.Beds.Select(b => new BedDetailDto
                    {
                        BedId = b.BedId,
                        BedNumber = b.BedNumber,
                        Status = b.Status,
                        TenantName = b.TenantName,
                        BedRent = b.BedRent
                    }).ToList()
                }).ToList()
            };
        }

        // 2. Dashboard Summary Cards
        public async Task<IEnumerable<FlatSummaryDto>> GetFlatCardsByUserIdAsync(Guid userId)
        {
            var flats = await _flatRepo.GetFlatsByUserIdAsync(userId);

            return flats.Select(f =>
            {
                var zones = f.Zones ?? new List<ZoneMaster>();
                var allBeds = zones.SelectMany(z => z.Beds ?? new List<BedMaster>()).ToList();

                bool isRoomWise = string.Equals(f.PricingType, "ROOM_WISE", StringComparison.OrdinalIgnoreCase);

                return new FlatSummaryDto
                {
                    Id = f.FlatId,
                    FlatNumber = f.FlatNumber,
                    ApartmentName = f.ApartmentName,
                    PricingType = f.PricingType,
                    TotalRooms = zones.Count,
                    TotalBeds = allBeds.Count,
                    OccupiedBeds = allBeds.Count(b => b.Status == enumBedStatus.Occupied || b.Status == enumBedStatus.Reserved),
                    VacantBeds = allBeds.Count(b => b.Status == enumBedStatus.Vacant),

                    // Expected Total Flat Rent Calculation
                    TotalFlatExpectedRent = isRoomWise
                        ? zones.Sum(z => z.RoomRent ?? 0)
                        : allBeds.Sum(b => b.BedRent),

                    // Mapping to your exact RoomBreakupDto structure
                    RoomBreakup = zones.Select(z =>
                    {
                        var roomBeds = z.Beds ?? new List<BedMaster>();
                        return new RoomBreakupDto
                        {
                            Id = z.ZoneId,
                            ZoneName = z.ZoneName,
                            Type = (int)z.Type,
                            Capacity = z.Capacity,
                            OccupiedBeds = roomBeds.Count(b => b.Status == enumBedStatus.Occupied || b.Status == enumBedStatus.Reserved),
                            VacantBeds = roomBeds.Count(b => b.Status == enumBedStatus.Vacant),
                            RoomRent = z.RoomRent ?? 0,

                            Beds = roomBeds.Select(b => new BedBreakupDto
                            {
                                Id = b.BedId, 
                                BedNumber = b.BedNumber,
                                Status = (int)b.Status,
                                BedRent = b.BedRent,
                                TenantName = b.TenantName
                            }).ToList()
                        };
                    }).ToList()
                };
            });
        }

        // 3. Create Flat with Zones & Beds
        public async Task CreateFlatAsync(CreateFlatDto dto)
        {
            _logger.LogInformation("Creating Flat: {FlatNumber} for User: {UserId}", dto.FlatNumber, dto.UserId);

            // 1. Check if PG Owner User exists
            var user = await _userRepo.GetById(dto.UserId);
            if (user == null)
                throw new KeyNotFoundException("PG Owner user not found.");

            // Null-Safe Flat Number parsing
            var cleanFlatNumber = dto.FlatNumber?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(cleanFlatNumber))
                throw new ArgumentException("Flat number is required.");

            // 2. Check duplicate flat number
            bool flatExists = await _flatRepo.IsFlatNumberExistsForOwnerAsync(cleanFlatNumber, dto.UserId);
            if (flatExists)
                throw new InvalidOperationException($"Flat number '{cleanFlatNumber}' already exists for this owner.");

            var flatId = Guid.NewGuid();

            // 3. Map Entity
            var flat = new FlatMaster
            {
                FlatId = flatId,
                FlatNumber = cleanFlatNumber,
                ApartmentName = dto.ApartmentName?.Trim() ?? string.Empty,
                UserId = dto.UserId,
                CreatedAt = DateTime.UtcNow,
                PricingType = string.IsNullOrWhiteSpace(dto.PricingType) ? "BED_WISE" : dto.PricingType,

                Zones = (dto.Zones ?? new List<CreateZoneDto>()).Select(z =>
                {
                    var zoneId = z.RealZoneId ?? Guid.NewGuid();

                    return new ZoneMaster
                    {
                        ZoneId = zoneId,
                        FlatId = flatId,
                        ZoneName = string.IsNullOrWhiteSpace(z.ZoneName) ? "Zone" : z.ZoneName.Trim(),
                        Type = (enumZoneType)z.Type,
                        Capacity = z.Capacity,
                        RoomRent = z.RealRent,

                        Beds = (z.Beds ?? new List<CreateBedDto>()).Select(b => new BedMaster
                        {
                            BedId = b.RealBedId ?? Guid.NewGuid(),
                            ZoneId = zoneId,
                            BedNumber = string.IsNullOrWhiteSpace(b.BedNumber) ? "Bed" : b.BedNumber.Trim(),
                            BedRent = b.RealRent,
                            Status = Enum.IsDefined(typeof(enumBedStatus), b.Status) ? b.Status : enumBedStatus.Vacant,
                            TenantName = b.TenantName ?? string.Empty,
                            UpdatedAt = DateTime.UtcNow
                        }).ToList()
                    };
                }).ToList()
            };

            // 4. Save to Database
            await _flatRepo.AddFlatAsync(flat);
        }

        // 4. Update Existing Flat
        // Update Existing Flat
        public async Task<bool> UpdateFlatAsync(Guid id, CreateFlatDto dto)
        {
            // 1. Load Existing Flat with Zones & Beds
            var existingFlat = await _flatRepo.GetFlatByIdAsync(id);
            if (existingFlat == null) return false;

            // 2. Basic Flat Details
            existingFlat.FlatNumber = dto.FlatNumber?.Trim() ?? existingFlat.FlatNumber;
            existingFlat.ApartmentName = dto.ApartmentName?.Trim() ?? existingFlat.ApartmentName;
            existingFlat.PricingType = dto.PricingType ?? existingFlat.PricingType;

            existingFlat.Zones ??= new List<ZoneMaster>();
            var incomingZones = dto.Zones ?? new List<CreateZoneDto>();

            // Database me existing DB IDs
            var dbBedIds = existingFlat.Zones
                .SelectMany(z => z.Beds ?? new List<BedMaster>())
                .Select(b => b.BedId)
                .ToHashSet();

            var dbZoneIds = existingFlat.Zones
                .Select(z => z.ZoneId)
                .ToHashSet();

            var incomingZoneIds = incomingZones
                .Select(z => z.RealZoneId)
                .Where(zId => zId.HasValue)
                .Select(zId => zId!.Value)
                .ToHashSet();

            // -------------------------------------------------------------
            // A. DELETE Zones removed from UI
            // -------------------------------------------------------------
            var zonesToRemove = existingFlat.Zones
                .Where(z => !incomingZoneIds.Contains(z.ZoneId))
                .ToList();

            foreach (var zone in zonesToRemove)
            {
                if (zone.Beds != null)
                {
                    foreach (var bed in zone.Beds.ToList())
                    {
                        _flatRepo.RemoveBed(bed);
                    }
                }
                _flatRepo.RemoveZone(zone);
                existingFlat.Zones.Remove(zone);
            }

            // -------------------------------------------------------------
            // B. ADD / UPDATE Zones & Beds
            // -------------------------------------------------------------
            foreach (var zoneDto in incomingZones)
            {
                var targetZoneId = zoneDto.RealZoneId;
                bool isExistingZone = targetZoneId.HasValue && dbZoneIds.Contains(targetZoneId.Value);

                if (isExistingZone)
                {
                    // UPDATE Zone
                    var existingZone = existingFlat.Zones.First(z => z.ZoneId == targetZoneId!.Value);

                    existingZone.ZoneName = string.IsNullOrWhiteSpace(zoneDto.ZoneName) ? "Zone" : zoneDto.ZoneName.Trim();
                    existingZone.Type = (enumZoneType)zoneDto.Type;
                    existingZone.Capacity = zoneDto.Capacity;
                    existingZone.RoomRent = zoneDto.RealRent;

                    SynchronizeBeds(existingZone, zoneDto.Beds, dbBedIds, isNewZone: false);
                }
                else
                {
                    // ADD NEW Zone
                    var newZone = new ZoneMaster
                    {
                        ZoneId = Guid.NewGuid(),
                        FlatId = id,
                        ZoneName = string.IsNullOrWhiteSpace(zoneDto.ZoneName) ? "Zone" : zoneDto.ZoneName.Trim(),
                        Type = (enumZoneType)zoneDto.Type,
                        Capacity = zoneDto.Capacity,
                        RoomRent = zoneDto.RealRent,
                        Beds = new List<BedMaster>()
                    };

                    _flatRepo.AddZone(newZone); // Force EF Core -> EntityState.Added
                    existingFlat.Zones.Add(newZone);

                    SynchronizeBeds(newZone, zoneDto.Beds, dbBedIds, isNewZone: true);
                }
            }

            // 3. Save Changes
            await _flatRepo.UpdateFlatAsync(existingFlat);
            return true;
        }

        private void SynchronizeBeds(ZoneMaster zone, List<CreateBedDto>? bedDtos, HashSet<Guid> dbBedIds, bool isNewZone = false)
        {
            zone.Beds ??= new List<BedMaster>();
            var incomingBeds = bedDtos ?? new List<CreateBedDto>();

            if (!isNewZone)
            {
                var incomingBedIds = incomingBeds
                    .Select(b => b.RealBedId)
                    .Where(bId => bId.HasValue)
                    .Select(bId => bId!.Value)
                    .ToHashSet();

                // Remove Beds deleted from UI
                var bedsToRemove = zone.Beds
                    .Where(b => !incomingBedIds.Contains(b.BedId))
                    .ToList();

                foreach (var bed in bedsToRemove)
                {
                    _flatRepo.RemoveBed(bed);
                    zone.Beds.Remove(bed);
                }
            }

            foreach (var bedDto in incomingBeds)
            {
                var targetBedId = bedDto.RealBedId;
                bool isExistingBedInDb = !isNewZone && targetBedId.HasValue && dbBedIds.Contains(targetBedId.Value);

                if (isExistingBedInDb)
                {
                    // UPDATE Existing Bed
                    var existingBed = zone.Beds.FirstOrDefault(b => b.BedId == targetBedId!.Value);
                    if (existingBed != null)
                    {
                        existingBed.BedNumber = string.IsNullOrWhiteSpace(bedDto.BedNumber) ? existingBed.BedNumber : bedDto.BedNumber.Trim();
                        existingBed.BedRent = bedDto.RealRent;
                        existingBed.Status = Enum.IsDefined(typeof(enumBedStatus), bedDto.Status) ? bedDto.Status : enumBedStatus.Vacant;
                        existingBed.TenantName = bedDto.TenantName ?? string.Empty;
                        existingBed.UpdatedAt = DateTime.UtcNow;
                    }
                }
                else
                {
                    // INSERT NEW Bed
                    var newBed = new BedMaster
                    {
                        BedId = Guid.NewGuid(), // Fresh GUID
                        ZoneId = zone.ZoneId,
                        BedNumber = string.IsNullOrWhiteSpace(bedDto.BedNumber) ? "Bed" : bedDto.BedNumber.Trim(),
                        BedRent = bedDto.RealRent,
                        Status = Enum.IsDefined(typeof(enumBedStatus), bedDto.Status) ? bedDto.Status : enumBedStatus.Vacant,
                        TenantName = bedDto.TenantName ?? string.Empty,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _flatRepo.AddBed(newBed); // 🔴 FORCE EF Core -> EntityState.Added (Ensures INSERT SQL query)
                    zone.Beds.Add(newBed);
                }
            }
        }

        // 5. Delete Flat (With occupied bed check)
        public async Task<(bool Success, string Message)> DeleteFlatAsync(Guid id)
        {
            var flat = await _flatRepo.GetFlatByIdAsync(id);
            if (flat == null)
                return (false, "Flat nahi mila.");

            bool isOccupied = await _flatRepo.HasOccupiedBedsAsync(id);
            if (isOccupied)
                return (false, "Is flat me occupied/reserved beds hain. Pehle unhe vacant karein ya shift karein.");

            await _flatRepo.DeleteFlatAsync(flat);
            return (true, "Flat successfully delete ho gaya.");
        }
    }
}