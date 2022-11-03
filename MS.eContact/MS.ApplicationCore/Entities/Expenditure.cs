using MS.ApplicationCore.MSEnums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Entities
{
    public class Expenditure: BaseEntity
    {
        public Guid ExpenditureId { get; set; }
        public string ExpenditureName { get; set; }
        public DateTime ExpenditureDate { get; set; }
        public ExpenditureType? ExpenditureType { get; set; }
        public string? Description { get; set; }
        public Guid? ExpenditurePlanId { get; set; }
        public Guid? ContactId { get; set; }
        public decimal Amount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

    }


}
