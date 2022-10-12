using System;
using System.Collections.Generic;
using System.Text;

namespace MS.ApplicationCore.Entities
{
    public class UserTokenConfirm
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string? TokenCode { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
