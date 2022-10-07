using System;
using System.Collections.Generic;
using System.Text;

namespace MS.ApplicationCore.Entities
{
    public class Event: BaseEntity
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventContent { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string EventPlace { get; set; }
        public string EventAddress { get; set; }
        public decimal Spends { get; set; }
        public string Note { get; set; }
        public bool? IsCancel { get; set; }
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

    }
}
