using MS.ApplicationCore.MSEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Entities
{
    public class ExpenditurePlan:BaseEntity
    {
        public Guid ExpenditurePlanId { get; set; }
        public string ExpenditurePlanName { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ExpenditurePlanType? ExpenditurePlanType { get; set; }
        public int? EventId { get; set; }
        public Guid? ContactId { get; set; }
        public bool? IsFinish { get; set; }
        public Guid? OrganizationId { get; set; }
        public decimal AmountUnit { get; set; }

    }
}
