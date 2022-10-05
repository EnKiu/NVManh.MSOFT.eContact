using System;
using System.Collections.Generic;
using System.Text;

namespace MS.ApplicationCore.Entities
{
    public class EventDetail: BaseEntity
    {
        public Guid EventDetailId { get; set; }
        public int EventId { get; set; }
        public Guid ContactId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public int? NumberAccompanying { get; set; }
        public string? Note { get; set; }
        public decimal? SpendsTotal { get; set; }
    }
}
