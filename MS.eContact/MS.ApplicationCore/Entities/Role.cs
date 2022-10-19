using MS.ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Entities.Auth
{
    public class Role:BaseEntity
    {
        public Guid RoleId { get; set; }
        public string? RoleName { get; set; }
        public Enums.Role RoleValue { get; set; }
        public string? OtherName { get; set; }
    }
}
