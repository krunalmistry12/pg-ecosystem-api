using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Application.DTOs.Expense
{
    public class CreateExpenseDto
    {
        public Guid? FlatId { get; set; } 
        public bool IsCommonExpense { get; set; } = false;

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 1000000)]
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

        public string? ReceiptName { get; set; }
        public string? ReceiptUri { get; set; }
        public string? Notes { get; set; }
    }
}
