using Dapper;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;
using MS.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MS.Infrastructure.Data
{
    public class EventRepository: DapperRepository<Event>, IEventRepository
    {
        IUnitOfWork _unitOfWork = null;
        string _tableName = string.Empty;
        public EventRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _tableName = typeof(Event).Name;
            _unitOfWork = unitOfWork;
        }
        public override IEnumerable<Event> All()
        {
            return _unitOfWork.Connection.Query<Event>($"SELECT * FROM Event Order By EventDate DESC", _unitOfWork.Transaction, commandType: System.Data.CommandType.Text);
        }
        public async override Task<IEnumerable<Event>> AllAsync()
        {
            return await _unitOfWork.Connection.QueryAsync<Event>($"SELECT * FROM Event Order By EventDate DESC", _unitOfWork.Transaction, commandType: System.Data.CommandType.Text);
        }

        public async Task<IEnumerable<Contact>> GetContactNotYetRegisterEventByEventId(int eventId)
        {
            var storeName = "Proc_Contact_GetContactNotRegisterEventByEventId";
            var parameters = new DynamicParameters();
            parameters.Add("@p_EventId", eventId);
            var data = await _unitOfWork.Connection.QueryAsync<Contact>(storeName, parameters,transaction: _unitOfWork.Transaction, commandType: System.Data.CommandType.StoredProcedure);
            return data;
        }
    }
}
