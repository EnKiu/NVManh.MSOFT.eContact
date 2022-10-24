using MS.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.DTOs
{
    public class EventInfo:Event
    {
        public string StartTimeText
        {
            get
            {
                return String.Format("{0:t}", StartTime); ;
            }
        }
        public string EndTimeText
        {
            get
            {
                return String.Format("{0:t}", EndTime); ;
            }
        }
        public List<EventDetail> EventDetails { get; set; }
        public int? TotalMember { get; set; }
        public int? TotalAccompanying { get; set; }
        public bool? NotRegisted { get; set; }
    }
}
