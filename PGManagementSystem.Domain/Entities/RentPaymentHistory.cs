using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PGManagementSystem.Domain.Enums;
namespace PGManagementSystem.Domain.Entities
{
    [Table("RENT_PAYMENTS_TRN")]
    public class RentPaymentHistory
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(50)]
        public string? ReceiptNumber { get; set; }
        [Required]
        public long RentId { get; set; }

        [ForeignKey("RentId")]
        public virtual RentMaster Rent { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountPaid { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; } = Global.GetIST();


        [MaxLength(30)]
        public enumPaymentMode PaymentMode { get; set; } = enumPaymentMode.UPI; // "UPI", "CASH", "BANK_TRANSFER"

        [MaxLength(100)]
        public string? TransactionId { get; set; }
        [Required]
        [MaxLength(20)]
        public string PaymentStatus { get; set; } = "SUCCESS";
        [MaxLength(255)]
        public string? Remarks { get; set; }

        public DateTime CreatedAt { get; set; } = Global.GetIST();
    }
}