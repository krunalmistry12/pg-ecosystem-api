using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PGManagementSystem.Domain.Enums;

namespace PGManagementSystem.Domain.Entities
{
    [Table("TENANTS_MST")]
    public class TenantMaster
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(15)]
        public string Phone { get; set; } = string.Empty;

        // ==========================================
        // PROPER FOREIGN KEYS & NAVIGATION PROPERTIES
        // ==========================================

        // 1. Link to FlatMaster (Property/Flat)
        [Required]
        [Column("PropertyId")] // FIX: Maps C# FlatId to MySQL PropertyId column
        public Guid FlatId { get; set; }

        [ForeignKey("FlatId")]
        public virtual FlatMaster? Flat { get; set; }

        // 2. Link to ZoneMaster (Room) - Optional
        public Guid? RoomId { get; set; }

        [ForeignKey("RoomId")]
        public virtual ZoneMaster? Room { get; set; }

        // 3. Link to BedMaster (Bed) - Optional
        public Guid? BedId { get; set; }

        [ForeignKey("BedId")]
        public virtual BedMaster? Bed { get; set; }

        // ==========================================
        // OTHER DETAILS
        // ==========================================

        [Required]
        public enumAllocationType AllocationType { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Rent { get; set; }

        [Required]
        public DateTime JoiningDate { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(15)]
        public string? EmergencyPhone { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Deposit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? AdvancePaid { get; set; }

        public int DueDate { get; set; } = 5;

        [MaxLength(20)]
        public string? PaymentMethod { get; set; }

        public double? StartingMeterReading { get; set; }

        public int LockInPeriodMonths { get; set; } = 6;

        public DateTime? AgreementEndDate { get; set; }

        [MaxLength(30)]
        public string? IdProofType { get; set; }

        [MaxLength(50)]
        public string? IdProofNumber { get; set; }

        public string? IdProofUrl { get; set; }

        public string? TenantPhotoUrl { get; set; }

        [MaxLength(20)]
        public string PoliceVerificationStatus { get; set; } = "NOT_STARTED";

        [Required]
        public enumTenantStatus Status { get; set; } = enumTenantStatus.ACTIVE;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? VacatedAt { get; set; }

        // TenantMaster class ke sabse niche ye line add karein:
        public virtual ICollection<RentMaster> Rents { get; set; } = new List<RentMaster>();
    }
}