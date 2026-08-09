using System.ComponentModel.DataAnnotations;

namespace PGManagementSystem.Application.DTOs.Rent;

public class GenerateRentBillDto
{
    [Required(ErrorMessage = "TenantId is required")]
    [Range(1, long.MaxValue, ErrorMessage = "Please select a valid Tenant")]
    public long TenantId { get; set; }

    [Required(ErrorMessage = "Month is required")]
    [Range(1, 12, ErrorMessage = "Month must be between 1 and 12")]
    public int Month { get; set; }

    [Required(ErrorMessage = "Year is required")]
    [Range(2020, 2100, ErrorMessage = "Please enter a valid Year")]
    public int Year { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Ending meter reading cannot be negative")]
    public decimal EndingMeterReading { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Rate per unit cannot be negative")]
    public decimal RatePerUnit { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Extra charges cannot be negative")]
    public decimal ExtraCharges { get; set; } = 0;

    [Range(0, double.MaxValue, ErrorMessage = "Discount cannot be negative")]
    public decimal Discount { get; set; } = 0;
}