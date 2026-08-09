using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PGManagementSystem.Domain.Enums;

namespace PGManagementSystem.Domain.Entities
{
    [Table("RENTS_MST")]
    public class RentMaster
    {
        [Key]
        public long Id { get; set; }

        // ==========================================
        // FOREIGN KEYS (Exact Types matching TenantMaster)
        // ==========================================
        [MaxLength(50)]
        public string? InvoiceNumber { get; set; }
        [Required]
        public long TenantId { get; set; }

        [ForeignKey("TenantId")]
        public virtual TenantMaster Tenant { get; set; } = null!;

        [Required]
        [Column("PropertyId")] // Matches C# FlatId to MySQL PropertyId column
        public Guid FlatId { get; set; }

        [ForeignKey("FlatId")]
        public virtual FlatMaster? Flat { get; set; }

        public Guid? RoomId { get; set; }

        [ForeignKey("RoomId")]
        public virtual ZoneMaster? Room { get; set; }

        public Guid? BedId { get; set; }

        [ForeignKey("BedId")]
        public virtual BedMaster? Bed { get; set; }

        // ==========================================
        // BILL DETAILS
        // ==========================================

        [Required]
        public int BillingMonth { get; set; } // e.g. 8 (August)

        [Required]
        public int BillingYear { get; set; }  // e.g. 2026

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BaseRent { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ElectricityBill { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal ExtraCharges { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal LateFee { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; set; } = 0;

        // Total Bill Amount Calculation
        [NotMapped]
        public decimal TotalAmount => (BaseRent + ElectricityBill + ExtraCharges + LateFee) - Discount;

        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidAmount { get; set; } = 0;

        [NotMapped]
        public decimal PendingAmount => TotalAmount - PaidAmount;

        // ==========================================
        // STATUS & DATES
        // ==========================================

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        [MaxLength(20)]
        public enumPaymentStatus Status { get; set; } = enumPaymentStatus.PENDING; // "PENDING", "PARTIAL", "PAID", "OVERDUE"
        public double? StartingMeterReading { get; set; } // Fixed: Added missing property
        public double? EndingMeterReading { get; set; }
        public double? UnitsConsumed { get; set; }

        public DateTime CreatedAt { get; set; } = Global.GetIST();
        public DateTime? UpdatedAt { get; set; }

        // Navigation property for partial payment records
        public virtual ICollection<RentPaymentHistory> PaymentHistories { get; set; } = new List<RentPaymentHistory>();
    }
}