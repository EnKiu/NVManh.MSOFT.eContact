﻿using MS.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Interfaces
{
    public interface IEventDetailRepository:IAsyncRepository<EventDetail>, IRepository<EventDetail>
    {
        public Task<bool> CheckRegisted(EventDetail eventDetail);
        public Task<IEnumerable<EventDetail>> GetRegisterEventByEventId(int eventId);
        Task<int> DeleteEventDetailByEventIdAndContactId(int eventId, Guid contactId);
    }
}
