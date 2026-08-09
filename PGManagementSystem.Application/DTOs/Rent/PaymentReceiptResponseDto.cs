using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Application.DTOs.Rent
{
    public class PaymentReceiptResponseDto
    {
        public string ReceiptNumber { get; set; } = string.Empty;
        public long RentId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public decimal AmountPaid { get; set; }
        public decimal RemainingPendingAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMode { get; set; } = string.Empty;
        public string? TransactionId { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
