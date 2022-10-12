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
        public EventRepository(MySqlDbContext dbContext) : base(dbContext)
        {
            _tableName = typeof(Event).Name;
        }
        public override IEnumerable<Event> All()
        {
            var storeName = "Proc_Event_GetEventInfo";
            return DbContext.Connection.Query<Event>(storeName, DbContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);
        }
        public async override Task<IEnumerable<Event>> AllAsync()
        {
            var storeName = "Proc_Event_GetEventInfo";
            return await DbContext.Connection.QueryAsync<Event>(storeName, DbContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);
            //return await DbContext.Connection.QueryAsync<Event>($"SELECT * FROM Event Order By EventDate DESC", DbContext.Transaction, commandType: System.Data.CommandType.Text);
        }

        public async Task<IEnumerable<Contact>> GetContactNotYetRegisterEventByEventId(int eventId)
        {
            var storeName = "Proc_Contact_GetContactNotRegisterEventByEventId";
            var parameters = new DynamicParameters();
            parameters.Add("@p_EventId", eventId);
            var data = await DbContext.Connection.QueryAsync<Contact>(storeName, parameters,transaction: DbContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);
            return data;
        }
        public override int Add(Event entity)
        {
            // Cập nhật thông tin ngày bắt đầu:
            entity.EventDate = (DateTime)entity.StartTime;

            var rowAffects = 0;
            DbContext.Connection.Open();
            DbContext.Connection.BeginTransaction();
            try
            {
                rowAffects = DbContext.Connection.Execute($"Proc_Event_InsertEvent", entity, transaction: DbContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);
                DbContext.Transaction.Commit();
            }
            catch (Exception)
            {
                DbContext.Transaction.Rollback();
            }
            DbContext.Connection.Close();
            return rowAffects;
        }

        public async override Task<int> AddAsync(Event entity)
        {
            // Cập nhật thông tin ngày bắt đầu:
            entity.EventDate = (DateTime)entity.StartTime;

            var rowAffects = 0;
            DbContext.Connection.Open();
            try
            {
                rowAffects = await DbContext.Connection.ExecuteAsync($"Proc_Event_InsertEvent", entity, transaction: DbContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);
                DbContext.Transaction.Commit();
            }
            catch (Exception)
            {
                DbContext.Transaction.Rollback();
            }
            DbContext.Connection.Close();
            return rowAffects;
        }
    }
}
