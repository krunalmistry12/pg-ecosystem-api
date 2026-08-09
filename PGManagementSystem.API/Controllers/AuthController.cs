using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PGManagementSystem.Application.DTOs.Verify;
using PGManagementSystem.Application.Interfaces; // 👈 IOtpService ke liye
using PGManagementSystem.Domain.Enums;
using PGManagementSystem.Infrastructure.Data;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PGManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;
    private readonly AuthService _authService;
    private readonly IOtpService _otpService; // 👈 1. Real OTP Service Inject ki

    public AuthController(
        IConfiguration configuration,
        AppDbContext context,
        AuthService authService,
        IOtpService otpService) // 👈 2. Constructor me Inject kiya
    {
        _configuration = configuration;
        _context = context;
        _authService = authService;
        _otpService = otpService;
    }

    // ==========================================
    // 1. SEND OTP STATELESS (With Real Delivery)
    // ==========================================
    [HttpPost("send-otp-stateless")]
    public async Task<IActionResult> SendOtpStateless([FromBody] SendOtpDto model) // 👈 async banaya
    {
        if (string.IsNullOrEmpty(model.Phone) || model.Phone.Length != 10)
        {
            return BadRequest(new { success = false, message = "Please provide a valid 10-digit mobile number." });
        }

        bool isActiveUser = false;
        string? userEmail = null;

        // A. Check in UserMasters (PG Owners / Admins / Managers)
        var ownerOrManager = _context.UserMasters
            .Include(u => u.Role)
            .FirstOrDefault(u => u.Phone == model.Phone);

        if (ownerOrManager != null)
        {
            if (!ownerOrManager.IsActive)
            {
                return StatusCode(403, new { success = false, message = "Your account is inactive. Please contact support." });
            }
            isActiveUser = true;
            userEmail = ownerOrManager.Email;
        }
        else
        {
            // B. Check in TenantMasters
            var tenant = _context.TenantMasters.FirstOrDefault(t => t.Phone == model.Phone);
            if (tenant != null)
            {
                if (tenant.Status != enumTenantStatus.ACTIVE)
                {
                    return StatusCode(403, new { success = false, message = "Your tenancy status is inactive." });
                }
                isActiveUser = true;
                userEmail = tenant.Email;
            }
        }

        if (!isActiveUser)
        {
            return NotFound(new { success = false, message = "Mobile number not registered or account is inactive." });
        }

        // C. Generate 6-Digit Random OTP
        string otp = new Random().Next(100000, 999999).ToString();

        // 👈 ADD-ON 1: Send REAL OTP via WhatsApp / SMS / Email
        bool isOtpSent = false;
        string selectedChannel = model.Channel?.ToLower() ?? "whatsapp"; // Default WhatsApp

        //switch (selectedChannel)
        //{
        //    case "whatsapp":
        //        isOtpSent = await _otpService.SendWhatsAppOtpAsync(model.Phone, otp);
        //        break;
        //    case "sms":
        //        isOtpSent = await _otpService.SendSmsOtpAsync(model.Phone, otp);
        //        break;
        //    case "email":
        //        if (!string.IsNullOrEmpty(userEmail))
        //            isOtpSent = await _otpService.SendEmailOtpAsync(userEmail, otp);
        //        break;
        //    default:
        //        isOtpSent = await _otpService.SendWhatsAppOtpAsync(model.Phone, otp);
        //        break;
        //}

        // Production Debug Output
        Debug.WriteLine($"[SEND OTP] Phone: {model.Phone} | Channel: {selectedChannel} | Generated OTP: {otp}");

        // D. Create Stateless OTP JWT Token (5 Minutes Validity)
        var secretKeyString = _configuration["Jwt:Key"] ?? "THIS_IS_MY_VERY_SECURE_SECRET_KEY_FOR_JWT_TOKEN_GENERATION_123456";
        var key = Encoding.UTF8.GetBytes(secretKeyString);

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {
                new Claim("phone", model.Phone),
                new Claim("otp", otp)
            }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var otpToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

        return Ok(new
        {
            success = true,
            message = $"OTP sent successfully via {selectedChannel.ToUpper()}.",
            otpToken = otpToken,
            // Testing env ke liye (Production me is OTP field ko hta sakte hain):
            debugOtp = otp
        });
    }

    // ==========================================
    // 2. VERIFY OTP STATELESS & LOGIN
    // ==========================================
    [HttpPost("verify-otp-stateless")]
    public IActionResult VerifyOtpStateless([FromBody] VerifyOtpDto model)
    {
        try
        {
            var secretKeyString = _configuration["Jwt:Key"] ?? "THIS_IS_MY_VERY_SECURE_SECRET_KEY_FOR_JWT_TOKEN_GENERATION_123456";
            var key = Encoding.UTF8.GetBytes(secretKeyString);

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(model.OtpToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            string? tokenPhone = principal.FindFirst("phone")?.Value;
            string? tokenOtp = principal.FindFirst("otp")?.Value;

            if (tokenPhone != model.Phone || tokenOtp != model.Otp)
            {
                return BadRequest(new { success = false, message = "Invalid OTP or Phone number." });
            }

            object? userData = null;
            string loginToken = string.Empty;

            // 1️⃣ Check in UserMasters (Admin / Owner)
            var user = _context.UserMasters
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Phone == model.Phone && u.IsActive);

            if (user != null)
            {
                loginToken = _authService.GenerateToken(user);

                userData = new
                {
                    id = user.UserId,
                    phone = user.Phone,
                    role = user.Role?.RoleName ?? "Admin",
                    name = user.FullName,
                    email = user.Email
                };
            }
            else
            {
                // 2️⃣ Check in TenantMasters
                var tenant = _context.TenantMasters
                    .FirstOrDefault(t => t.Phone == model.Phone && t.Status == enumTenantStatus.ACTIVE);

                if (tenant != null)
                {
                    // 👈 Tenant Token Generator Call
                    loginToken = _authService.GenerateTokenForTenant(tenant);

                    userData = new
                    {
                        id = tenant.Id,
                        phone = tenant.Phone,
                        role = "Tenant",
                        name = tenant.Name,
                        email = tenant.Email
                    };
                }
                else
                {
                    return NotFound(new { success = false, message = "User not found or account inactive." });
                }
            }

            return Ok(new
            {
                success = true,
                message = "Login successful",
                token = loginToken,
                user = userData
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[VERIFY ERROR]: {ex.Message}");
            return BadRequest(new { success = false, message = "OTP has expired or token is invalid." });
        }
    }
}