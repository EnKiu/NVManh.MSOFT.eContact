using MS.ApplicationCore.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.ApplicationCore.Entities
{
    public class Event: BaseEntity
    {
        public Event()
        {
            if (ExpireRegisterDate == null)
            {
                ExpireRegisterDate = EventDate;
            }
        }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventContent { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? ExpireRegisterDate { get; set; }
        public string EventPlace { get; set; }
        public string EventAddress { get; set; }
        public decimal Spends { get; set; }
        public string Note { get; set; }
        public bool? IsCancel { get; set; }

        [NotMapQuery]
        public string StartTimeText
        {
            get
            {
                return String.Format("{0:t}", StartTime); ;
            }
        }

        [NotMapQuery]
        public string EndTimeText
        {
            get
            {
                return String.Format("{0:t}", EndTime); ;
            }
        }

        [NotMapQuery]
        public List<EventDetail> EventDetails { get; set; }

        [NotMapQuery]
        public int? TotalMember { get; set; }

        [NotMapQuery]
        public int? TotalAccompanying { get; set; }

        [NotMapQuery]
        public bool? NotRegisted { get; set; }

        [NotMapQuery]
        public decimal? SpendsTotal { get; set; }

    }
}
