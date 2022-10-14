using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Entities.Auth
{
    public class UserClaim
    {
        public Guid UserClaimId { get; set; }
        public Guid UserId { get; set; }
        public string? ClaimType { get; set; }
        public string? ClaimValue { get; set; }
    }
}
