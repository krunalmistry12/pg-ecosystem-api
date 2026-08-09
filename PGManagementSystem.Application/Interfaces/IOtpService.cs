namespace PGManagementSystem.Application.Interfaces
{
    public interface IOtpService
    {
        string GenerateOtp(int length = 6);
        Task<bool> SendEmailOtpAsync(string email, string otp);
        Task<bool> SendSmsOtpAsync(string phoneNumber, string otp);
        Task<bool> SendWhatsAppOtpAsync(string phoneNumber, string otp);
    }
}