﻿using MS.ApplicationCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Entities.Auth
{
    public class AspNetUsers: BaseEntity
    {
        public AspNetUsers()
        {
            Roles = new List<AspNetRoles>();
        }

        public Guid Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int EmailConfirmed { get; set; } = 0;
        public int PhoneNumberConfirmed { get; set; } = 0;
        public int AccessFailedCount { get; set; } = 0;
        public DateTime? LockoutEndDateUtc { get; set; }
        public string? SecurityStamp { get; set; }
        public int LockoutEnabled { get; set; } = 0;
        public IEnumerable<AspNetRoles> Roles { get; set; }

        [JsonIgnore]
        public string? PasswordHash { get; set; }

        //[JsonIgnore]
        public string? Password { get; set; }

        public string? RoleName { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }

    }
}
