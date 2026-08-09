using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Domain
{
    public static class Global
    {
        public static DateTime GetIST()
        {
            TimeZoneInfo istZone;
            try
            {
                istZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata"); // Linux / Docker / Cloud
            }
            catch
            {
                istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"); // Windows
            }
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, istZone);
        }
    }
}
