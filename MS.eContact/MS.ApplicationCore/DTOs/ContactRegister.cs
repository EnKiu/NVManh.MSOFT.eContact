using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.DTOs
{
    public class ContactRegister
    {
        public Guid ContactId { get; set; }
        public string? FullName { get; set; }
        public Guid? OrganizationId { get; set; }
    }
}
