using MS.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.DTOs
{
    public class RegisterEventInfo:EventDetail
    {
        public bool NotYetPaid { get; set; }
        public decimal? Amount { get; set; }
        public Guid? ExpenditureId { get; set; }
        public Guid? ExpenditurePlanId { get; set; }
    }
}
