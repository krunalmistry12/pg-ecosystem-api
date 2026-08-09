using System;
using Microsoft.AspNetCore.Http;
using PGManagementSystem.Domain.Enums;

namespace PGManagementSystem.Application.DTOs
{
    public class UpdateTenantDto
    {
        // ==========================================
        // 1. LOCATION / ROOM SWITCHING DETAILS (ADDED)
        // ==========================================
        public Guid? FlatId { get; set; }           // Agar kisi doosre Flat/Property mein shift karna ho
        public enumAllocationType? AllocationType { get; set; } // E.g. Single Bed se Full Room
        public Guid? RoomId { get; set; }           // Naya Room
        public Guid? BedId { get; set; }            // Naya Bed

        // ==========================================
        // 2. BASIC TENANT INFO UPDATE
        // ==========================================
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? EmergencyPhone { get; set; }
        public string? FatherName { get; set; }
        public string? PermanentAddress { get; set; }
        public string? Occupation { get; set; }

        // ==========================================
        // 3. FINANCIAL & RENT UPDATES
        // ==========================================
        public decimal? Rent { get; set; }           // Naye room ka badla hua rent
        public decimal? Deposit { get; set; }
        public int? DueDate { get; set; }
        public string? PaymentMethod { get; set; }

        // ==========================================
        // 4. POLICE VERIFICATION & PROOFS
        // ==========================================
        public string? PoliceVerificationStatus { get; set; } // NOT_STARTED, PENDING, VERIFIED, REJECTED
        public string? PoliceVerificationNumber { get; set; }
        public string? IdProofType { get; set; }
        public string? IdProofNumber { get; set; }

        // ==========================================
        // 5. PHYSICAL FILE UPLOADS (OPTIONAL)
        // ==========================================
        public IFormFile? PoliceVerificationDocFile { get; set; }
        public IFormFile? IdProofFile { get; set; }
        public IFormFile? TenantPhotoFile { get; set; }
        public IFormFile? AgreementDocFile { get; set; }
    }
}