using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Application.DTOs.Rent
{
    public class RecordPaymentDto
    {
        [Required(ErrorMessage = "Rent ID is required.")]
        public long RentId { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount paid must be greater than zero.")]
        public decimal AmountPaid { get; set; }

        [Required(ErrorMessage = "Payment mode is required (e.g., UPI, CASH, BANK_TRANSFER).")]
        public string PaymentMode { get; set; } = "UPI";

        public string? TransactionId { get; set; }

        public string? Remarks { get; set; }
    }
}
