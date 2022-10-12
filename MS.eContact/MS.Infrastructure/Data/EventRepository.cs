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
        string _tableName = string.Empty;
        public EventRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _tableName = typeof(Event).Name;
            UnitOfWork = unitOfWork;
        }
        public override IEnumerable<Event> All()
        {
            var storeName = "Proc_Event_GetEventInfo";
            return UnitOfWork.Connection.Query<Event>(storeName, UnitOfWork.Transaction, commandType: System.Data.CommandType.StoredProcedure);
        }
        public async override Task<IEnumerable<Event>> AllAsync()
        {
            return await UnitOfWork.Connection.QueryAsync<Event>($"SELECT * FROM Event Order By EventDate DESC", UnitOfWork.Transaction, commandType: System.Data.CommandType.Text);
        }

        public async Task<IEnumerable<Contact>> GetContactNotYetRegisterEventByEventId(int eventId)
        {
            var storeName = "Proc_Contact_GetContactNotRegisterEventByEventId";
            var parameters = new DynamicParameters();
            parameters.Add("@p_EventId", eventId);
            var data = await UnitOfWork.Connection.QueryAsync<Contact>(storeName, parameters,transaction: UnitOfWork.Transaction, commandType: System.Data.CommandType.StoredProcedure);
            return data;
        }
        public override int Add(Event entity)
        {
            // Cập nhật thông tin ngày bắt đầu:
            entity.EventDate = (DateTime)entity.StartTime;

            var rowAffects = 0;
            UnitOfWork.Connection.Open();
            UnitOfWork.BeginTransaction();
            try
            {
                rowAffects = UnitOfWork.Connection.Execute($"Proc_Event_InsertEvent", entity, transaction: UnitOfWork.Transaction, commandType: System.Data.CommandType.StoredProcedure);
                UnitOfWork.Commit();
            }
            catch (Exception)
            {
                UnitOfWork.Rollback();
            }
            UnitOfWork.Connection.Close();
            return rowAffects;
        }

        public async override Task<int> AddAsync(Event entity)
        {
            // Cập nhật thông tin ngày bắt đầu:
            entity.EventDate = (DateTime)entity.StartTime;

            var rowAffects = 0;
            UnitOfWork.Connection.Open();
            UnitOfWork.BeginTransaction();
            try
            {
                rowAffects = await UnitOfWork.Connection.ExecuteAsync($"Proc_Event_InsertEvent", entity, transaction: UnitOfWork.Transaction, commandType: System.Data.CommandType.StoredProcedure);
                UnitOfWork.Commit();
            }
            catch (Exception)
            {
                UnitOfWork.Rollback();
            }
            UnitOfWork.Connection.Close();
            return rowAffects;
        }
    }
}
