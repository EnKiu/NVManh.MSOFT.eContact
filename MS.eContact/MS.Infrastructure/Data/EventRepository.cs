using Dapper;
using Microsoft.AspNetCore.Http;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;
using MS.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Infrastructure.Data
{
    public class EventRepository : DapperRepository<Event>, IEventRepository
    {
        string _tableName = string.Empty;
        private readonly  IHttpContextAccessor _httpContextAccessor;
        public EventRepository(MySqlDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext)
        {
            _tableName = typeof(Event).Name;
            _httpContextAccessor = httpContextAccessor;
        }
        public override IEnumerable<Event> All()
        {
            var storeName = "Proc_Event_GetEventInfo";
            return DbContext.Connection.Query<Event>(storeName, DbContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);
        }
        public async override Task<IEnumerable<Event>> AllAsync()
        {
            //var userId = _httpContextAccessor.HttpContext.User?.Claims?.First(x => x.Type == "id").Value;
            var storeName = "Proc_Event_GetEventInfo_ByUserId";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("p_UserId", null);
            return await DbContext.Connection.QueryAsync<Event>(storeName,param:parameters, DbContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);
            //return await DbContext.Connection.QueryAsync<Event>($"SELECT * FROM Event Order By EventDate DESC", DbContext.Transaction, commandType: System.Data.CommandType.Text);
        }

        public async Task<IEnumerable<Contact>> GetContactNotYetRegisterEventByEventId(int eventId)
        {
            
            var storeName = "Proc_Contact_GetContactNotRegisterEventByEventId";
            var parameters = new DynamicParameters();
            parameters.Add("@p_EventId", eventId);
            var data = await DbContext.Connection.QueryAsync<Contact>(storeName, parameters, transaction: DbContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);
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
            var rowAffects = await DbContext.Connection.ExecuteAsync($"Proc_Event_InsertEvent", entity, transaction: DbContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);
            return rowAffects;
        }

        public async Task<int> DeleteEventDetailByEventIdAndUserId(int eventId, string userId)
        {
            var storeName = "Proc_EventDetail_DeleteEventDetailByEventIdAndUserId";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@p_EventId",eventId);
            parameters.Add("@p_UserId", userId);
            var res = await DbContext.Connection.ExecuteAsync(storeName, param: parameters, transaction: DbContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);
            return res;
        }
    }
}
