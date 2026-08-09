using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Application.DTOs.Rent
{
    public class ElectricityBreakdownDto
    {
        public double StartingMeterReading { get; set; }
        public double EndingMeterReading { get; set; }
        public double UnitsConsumed { get; set; }
        public decimal TotalElectricityBill { get; set; }
    }
}
