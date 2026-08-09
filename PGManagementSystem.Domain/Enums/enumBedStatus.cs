using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Domain.Enums
{
    public enum enumBedStatus
    {
        Vacant = 1,      // Khali bed
        Occupied = 2,    // Booked/Tenant reh raha hai
        Reserved = 3,    // Advance booking done
        Maintenance = 4  // Repairing/Unavailable
    }
}
