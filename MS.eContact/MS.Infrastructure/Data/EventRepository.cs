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
            var storeName = "Proc_Event_GetEventInfo";
            return _unitOfWork.Connection.Query<Event>(storeName, _unitOfWork.Transaction, commandType: System.Data.CommandType.StoredProcedure);
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
        public override int Add(Event entity)
        {
            // Cập nhật thông tin ngày bắt đầu:
            entity.EventDate = (DateTime)entity.StartTime;

            var rowAffects = 0;
            _unitOfWork.Connection.Open();
            _unitOfWork.Begin();
            try
            {
                rowAffects = _unitOfWork.Connection.Execute($"Proc_Event_InsertEvent", entity, commandType: System.Data.CommandType.StoredProcedure);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
            }
            _unitOfWork.Connection.Close();
            return rowAffects;
        }

        public async override Task<int> AddAsync(Event entity)
        {
            // Cập nhật thông tin ngày bắt đầu:
            entity.EventDate = (DateTime)entity.StartTime;

            var rowAffects = 0;
            _unitOfWork.Connection.Open();
            _unitOfWork.Begin();
            try
            {
                rowAffects = await _unitOfWork.Connection.ExecuteAsync($"Proc_Event_InsertEvent", entity, commandType: System.Data.CommandType.StoredProcedure);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
            }
            _unitOfWork.Connection.Close();
            return rowAffects;
        }
    }
}
