using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Application.DTOs
{
    public class UpdateFlatDto
    {
        public string FlatNumber { get; set; } = string.Empty;
        public string ApartmentName { get; set; } = string.Empty;
        public string PricingType { get; set; } = "BED_WISE";
    }
}
