using System;
using System.Net;
using System.Net.Mail;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PGManagementSystem.Application.Interfaces;

namespace PGManagementSystem.Infrastructure.Services
{
    public class OtpService : IOtpService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public OtpService(IConfiguration config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        // 🎲 1. Random 6-Digit OTP Generator
        public string GenerateOtp(int length = 6)
        {
            Random random = new Random();
            string otp = "";
            for (int i = 0; i < length; i++)
            {
                otp += random.Next(0, 10).ToString();
            }
            return otp;
        }

        // 📧 2. Send OTP via Email (SMTP - Gmail / SendGrid)
        public async Task<bool> SendEmailOtpAsync(string email, string otp)
        {
            try
            {
                var smtpHost = _config["EmailSettings:Host"] ?? "smtp.gmail.com";
                var smtpPort = int.Parse(_config["EmailSettings:Port"] ?? "587");
                var senderEmail = _config["EmailSettings:Email"];
                var senderPassword = _config["EmailSettings:Password"];

                using var client = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(senderEmail, senderPassword),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail!, "PG Management System"),
                    Subject = "Your Login Verification OTP",
                    Body = $@"
                <div style='font-family: Arial, sans-serif; padding: 20px; border: 1px solid #e0e0e0; border-radius: 8px;'>
                    <h2 style='color: #2c3e50;'>PG Management System</h2>
                    <p>Hello,</p>
                    <p>Your OTP for account login is:</p>
                    <h1 style='color: #e74c3c; letter-spacing: 4px;'>{otp}</h1>
                    <p>This OTP is valid for <b>5 minutes</b>. Please do not share it with anyone.</p>
                </div>",
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email);
                await client.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EMAIL ERROR]: {ex.Message}");
                return false;
            }
        }
        // 📱 3. Send OTP via WhatsApp (Meta WhatsApp Cloud API)
        public async Task<bool> SendWhatsAppOtpAsync(string phoneNumber, string otp)
        {
            try
            {
                // Phone format fix (e.g., 917894561231)
                string formattedPhone = phoneNumber.StartsWith("91") ? phoneNumber : $"91{phoneNumber}";

                var phoneNumberId = _config["WhatsApp:PhoneNumberId"];
                var accessToken = _config["WhatsApp:AccessToken"];

                string url = $"https://graph.facebook.com/v18.0/{phoneNumberId}/messages";

                // Meta Official Template Body Format
                var payload = new
                {
                    messaging_product = "whatsapp",
                    to = formattedPhone,
                    type = "template",
                    template = new
                    {
                        name = "otp_verification", // Aapke Meta Manager me approved template name
                        language = new { code = "en_US" },

                        // 👈 FIX: 'new[]' ko 'new object[]' se replace kiya error CS0826 hatane ke liye
                        components = new object[]
                        {
                            new
                            {
                                type = "body",
                                parameters = new object[]
                                {
                                    new { type = "text", text = otp }
                                }
                            },
                            new
                            {
                                type = "button",
                                sub_type = "url",
                                index = "0",
                                parameters = new object[]
                                {
                                    new { type = "text", text = otp }
                                }
                            }
                        }
                    }
                };

                var jsonPayload = JsonSerializer.Serialize(payload);
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("Authorization", $"Bearer {accessToken}");
                request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WhatsApp OTP Error: {ex.Message}");
                return false;
            }
        }

        // 💬 4. Send OTP via SMS (Fast2SMS / Twilio)
        public async Task<bool> SendSmsOtpAsync(string phoneNumber, string otp)
        {
            try
            {
                // Fast2SMS (India) Example
                string apiKey = _config["SmsSettings:ApiKey"]!;
                string url = $"https://www.fast2sms.com/dev/bulkV2?authorization={apiKey}&route=otp&variables_values={otp}&numbers={phoneNumber}";

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SMS OTP Error: {ex.Message}");
                return false;
            }
        }
    }
}