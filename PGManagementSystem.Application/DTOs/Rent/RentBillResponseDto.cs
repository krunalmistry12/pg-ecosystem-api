using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGManagementSystem.Domain.Enums;

namespace PGManagementSystem.Application.DTOs.Rent
{
    public class RentBillResponseDto
    {
        public long RentId { get; set; }
        public string? InvoiceNumber { get; set; }
        public long TenantId { get; set; }
        public string TenantName { get; set; } = string.Empty;
        public string TenantPhone { get; set; } = string.Empty;
        public int BillingMonth { get; set; }
        public int BillingYear { get; set; }
        public decimal BaseRent { get; set; }
        public decimal ElectricityBill { get; set; }
        public decimal ExtraCharges { get; set; }
        public decimal LateFee { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PendingAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }

        public ElectricityBreakdownDto ElectricityDetails { get; set; } = new();
    }
}
