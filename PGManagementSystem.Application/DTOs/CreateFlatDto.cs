using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace PGManagementSystem.Application.DTOs
{
    public class CreateFlatDto
    {
        [Required(ErrorMessage = "Flat number is required")]
        public string FlatNumber { get; set; } = string.Empty; // e.g. "101"

        [Required(ErrorMessage = "Apartment name is required")]
        public string ApartmentName { get; set; } = string.Empty; // e.g. "Roma Apartment"

        [Required(ErrorMessage = "UserId (Owner ID) is required")]
        public Guid UserId { get; set; } // PG Owner Guid
        public string PricingType { get; set; } = "BED_WISE";
        public List<CreateZoneDto> Zones { get; set; } = new List<CreateZoneDto>();
    }
}
