using MS.ApplicationCore.Helpers;
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

        [NotMapQuery]
        public string LastName { get; set; }

        [NotMapQuery]
        public string FirstName { get; set; }

        [NotMapQuery]
        public string FullName { get; set; }
        public int? NumberAccompanying { get; set; }
        public string? Note { get; set; }
        public decimal? SpendsTotal { get; set; }
    }
}
