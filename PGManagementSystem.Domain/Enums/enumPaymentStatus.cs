using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Domain.Enums
{
    public enum enumPaymentStatus
    {
        PAID = 1,
        PENDING = 2,
        PARTIAL = 3,
        
        OVERDUE = 4 
    }
}
