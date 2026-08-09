using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using PGManagementSystem.Domain.Enums;

namespace PGManagementSystem.Application.DTOs
{
    public class CreateTenantDto
    {
        // ==========================================
        // 1. MANDATORY FIELDS (FAST CREATION)
        // ==========================================

        [Required(ErrorMessage = "Tenant name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be 10 digits")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Flat/Property ID is required")]
        public Guid FlatId { get; set; }

        [Required(ErrorMessage = "Allocation type is required")]
        public enumAllocationType AllocationType { get; set; } = enumAllocationType.FULL_FLAT;

        [Required(ErrorMessage = "Rent amount is required")]
        [Range(1, double.MaxValue, ErrorMessage = "Rent must be greater than 0")]
        public decimal Rent { get; set; }

        [Required(ErrorMessage = "Joining date is required")]
        public DateTime JoiningDate { get; set; } = DateTime.UtcNow;

        // ==========================================
        // 2. OPTIONAL FIELDS (SERVICE DEFAULTS READY)
        // ==========================================

        public Guid? RoomId { get; set; }
        public Guid? BedId { get; set; }

        public string? Email { get; set; }
        public string? EmergencyPhone { get; set; }

        public decimal? Deposit { get; set; }
        public decimal? AdvancePaid { get; set; }
        public int DueDate { get; set; } = 5;

        public string? PaymentMethod { get; set; }
        public double? StartingMeterReading { get; set; }
        public int LockInPeriodMonths { get; set; } = 6;

        public string? IdProofType { get; set; }
        public string? IdProofNumber { get; set; }

        // ==========================================
        // 3. OPTIONAL FILE UPLOADS
        // ==========================================

        public IFormFile? IdProofFile { get; set; }
        public IFormFile? TenantPhotoFile { get; set; }
    }
}