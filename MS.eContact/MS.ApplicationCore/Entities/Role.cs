﻿using MS.ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Entities.Auth
{
    public class Role:BaseEntity
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Enums.Role RoleValue { get; set; }
        public string? OtherName { get; set; }
    }
}
