using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Domain.Entities
{
    [Table("EXPENSE_MST")]
    public class ExpenseMaster
    {
        [Key]
        public Guid ExpenseId { get; set; } = Guid.NewGuid();

        public Guid? FlatId { get; set; }

        [ForeignKey("FlatId")]
        public FlatMaster? Flat { get; set; }

        [Required]
        public bool IsCommonExpense { get; set; } = false;

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public UserMaster? CreatedByUser { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(20)]
        public string Month { get; set; } = string.Empty; 

        public DateTime Date { get; set; } = DateTime.UtcNow;

        [MaxLength(50)]
        public string PaymentMode { get; set; } = "UPI"; 

        [MaxLength(150)]
        public string PaidBy { get; set; } = string.Empty; 

        [MaxLength(50)]
        public string Status { get; set; } = "Paid"; 

        [MaxLength(255)]
        public string? ReceiptName { get; set; }

        [MaxLength(500)]
        public string? ReceiptUri { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = Global.GetIST();
    }
}
