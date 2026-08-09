using System;

namespace PGManagementSystem.Application.DTOs
{
    public class TenantResponseDto
    {
        public string Id { get; set; } = string.Empty;

        // ==========================================
        // 1. LOCATION & PROPERTY INFO
        // ==========================================
        public string FlatId { get; set; } = string.Empty;
        public string ApartmentName { get; set; } = string.Empty;
        public string FlatNumber { get; set; } = string.Empty;

        public string? RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;

        public string? BedId { get; set; }
        public string BedName { get; set; } = string.Empty;

        // ==========================================
        // 2. BASIC TENANT INFO
        // ==========================================
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? EmergencyPhone { get; set; }

        // ==========================================
        // 3. STATUS & ALLOCATION
        // ==========================================
        public string Status { get; set; } = "ACTIVE";
        public string AllocationType { get; set; } = "BED";
        public string PoliceVerificationStatus { get; set; } = "NOT_STARTED";

        // ==========================================
        // 4. FINANCIAL DETAILS (All Input Fields Included)
        // ==========================================
        public decimal Rent { get; set; }
        public decimal Deposit { get; set; }
        public decimal AdvancePaid { get; set; }    
        public int DueDate { get; set; }
        public string? PaymentMethod { get; set; }

        // ==========================================
        // 5. ADDITIONAL UTILITIES & DATES
        // ==========================================
        public double? StartingMeterReading { get; set; }
        public int LockInPeriodMonths { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime? AgreementEndDate { get; set; }
        public DateTime CreatedAt { get; set; }

        // ==========================================
        // 6. MEDIA & ID PROOFS
        // ==========================================
        public string? IdProofType { get; set; }
        public string? IdProofNumber { get; set; }
        public string? IdProofUrl { get; set; }
        public string? TenantPhotoUrl { get; set; }
    }
}