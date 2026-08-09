using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Application.DTOs
{
    public class BedBreakupDto
    {
        public Guid Id { get; set; }             
        public string BedNumber { get; set; } = string.Empty;
        public int Status { get; set; }        
        public decimal BedRent { get; set; }
        public string? TenantName { get; set; } 
    }
}
