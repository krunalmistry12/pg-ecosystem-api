using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Application.DTOs.Verify
{
    public class SendOtpDto
    {
        public string Phone { get; set; }
        public string? Channel { get; set; } = "whatsapp";
    }
}
