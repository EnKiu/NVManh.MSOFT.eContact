using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Entities.Auth;
using MS.ApplicationCore.MSEnums;
using MS.ApplicationCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MS.ApplicationCore.DTOs
{
    public class UserInfo
    {
        public UserInfo()
        {
            Roles = new List<Role>();
        }

        public Guid UserId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int EmailConfirmed { get; set; } = 0;
        public int PhoneNumberConfirmed { get; set; } = 0;
        public int AccessFailedCount { get; set; } = 0;
        public DateTime? LockoutEndDateUtc { get; set; }
        public string? SecurityStamp { get; set; }
        public int LockoutEnabled { get; set; } = 0;
        public IEnumerable<Role> Roles { get; set; }

        [JsonIgnore]
        public string? PasswordHash { get; set; }

        public MSRole? HighestRole { get; set; }
        public string? RoleName { get; set; }

        //public string? RefreshToken { get; set; }

        //public DateTime? RefreshTokenExpiryTime { get; set; }
        public Guid ContactId { get; set; }
        public Contact Contact { get; set; }
        public string? AvatarLink { get; set; }
        public string? AvatarFullPath
        {
            get
            {
                var random = new Random();
                if (AvatarLink != null)
                    return String.Format("{0}/{1}?v={2}", CommonConst.ServerFileUrl, AvatarLink, random.Next(1, 999));
                return null;
            }
        }
    }
}
