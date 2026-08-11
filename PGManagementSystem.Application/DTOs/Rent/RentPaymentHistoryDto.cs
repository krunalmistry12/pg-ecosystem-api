using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Application.DTOs.Rent
{
    public class RentPaymentHistoryDto
    {
        public long PaymentId { get; set; }
        public string? ReceiptNumber { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMode { get; set; } = string.Empty; // Enum ko string convert karne ke liye
        public string? TransactionId { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public string? Remarks { get; set; }
    }
}
