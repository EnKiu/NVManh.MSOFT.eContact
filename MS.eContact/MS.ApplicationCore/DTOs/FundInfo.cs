using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.DTOs
{
    public class FundInfo
    {
        public decimal? RevenueTotal { get; set; }
        public decimal? ExpenditureTotal { get; set; }
        public decimal? FundLeft { get; set; }
    }
}
