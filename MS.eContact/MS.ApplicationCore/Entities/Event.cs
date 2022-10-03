using System;
using System.Collections.Generic;
using System.Text;

namespace MS.ApplicationCore.Entities
{
    public class Event
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

    }

    public class EventDetail
    {
        public Guid EventDetailId { get; set; }
        public int EventId { get; set; }
        public Guid ContactId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + ' ' + LastName;
            }
            set { }
        }
        public int NumberAccompanying { get; set; }
        public string Note { get; set; }
        public decimal SpendsTotal { get; set; }
    }
}
