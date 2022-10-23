using MS.ApplicationCore.MSEnums;
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
        public MSEnums.MSRole RoleValue { get; set; }
        public string? OtherName { get; set; }
    }
}
