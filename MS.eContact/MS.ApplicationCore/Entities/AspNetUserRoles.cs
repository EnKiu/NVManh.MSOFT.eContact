using MS.ApplicationCore.Enums;
using MS.ApplicationCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Entities.Auth
{
    public class AspNetUserRoles:BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        [NotMapQuery]
        public Role? RoleValue { get; set; }
    }
}
