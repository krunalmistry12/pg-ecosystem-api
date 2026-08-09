using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Application.DTOs.Verify
{
    public class VerifyOtpDto
    {
        public string Phone { get; set; }
        public string Otp { get; set; }
        public string OtpToken { get; set; }
    }
}
