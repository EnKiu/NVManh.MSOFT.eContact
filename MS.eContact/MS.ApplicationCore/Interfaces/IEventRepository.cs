using MS.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Interfaces
{
    public interface IEventRepository: IAsyncRepository<Event>, IRepository<Event>
    {
        Task<IEnumerable<Contact>> GetContactNotYetRegisterEventByEventId(int eventId);

        Task<int> DeleteEventDetailByEventIdAndUserId(int eventId, string userId);

        Task<IEnumerable<Event>> GetEventLeftTime();
    }
}
