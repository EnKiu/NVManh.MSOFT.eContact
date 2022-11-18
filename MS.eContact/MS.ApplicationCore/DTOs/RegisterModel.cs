using System;
using System.Collections.Generic;
using System.Text;

namespace MS.ApplicationCore.Entities
{
    public class RegisterModel
    {
        public string UserName { get; set; }

        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
        public Guid ContactId { get; set; }
        public Guid OrganizationId { get; set; }

    }
}
