using System;
using System.Collections.Generic;
using System.Text;

namespace MS.ApplicationCore.DTOs
{
    public class UserRegisterResponse
    {
        public Guid? UserId { get; set; }
        public Guid? ContactId { get; set; }
        public string UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? MobileNumber { get; set; }

    }
}
