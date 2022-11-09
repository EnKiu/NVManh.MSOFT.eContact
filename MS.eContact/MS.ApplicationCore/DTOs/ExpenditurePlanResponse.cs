using MS.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.DTOs
{
    public class ExpenditurePlanResponse:ExpenditurePlan
    {
        public decimal TotalMoney { get; set; }
    }
}
