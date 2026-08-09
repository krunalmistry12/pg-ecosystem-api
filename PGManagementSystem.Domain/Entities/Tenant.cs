using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Domain.Entities
{
    public class Tenant
    {
        public int TenantId { get; set; }

        public int UserId { get; set; }   // FK

        public int PGId { get; set; }   

        public string Phone { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
