using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGManagementSystem.Domain.Enums;

namespace PGManagementSystem.Application.DTOs
{
    public class BedDetailDto
    {
        public Guid? BedId { get; set; }
        public string BedNumber { get; set; } = string.Empty;
        public enumBedStatus Status { get; set; }
        public string? TenantName { get; set; }
        public decimal BedRent { get; set; }
    }
}
