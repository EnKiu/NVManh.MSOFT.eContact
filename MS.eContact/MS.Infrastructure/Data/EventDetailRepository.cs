﻿using Dapper;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;
using MS.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MS.Infrastructure.Data
{
    public class EventDetailRepository:DapperRepository<EventDetail>, IEventDetailRepository
    {
        public EventDetailRepository(MySqlDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> CheckRegisted(EventDetail eventDetail)
        {
            var sqlCheck = "SELECT * FROM EventDetail e WHERE e.ContactId = @ContactId AND e.EventId = @EventId";
            var parameters = new DynamicParameters();
            parameters.Add("@ContactId", eventDetail.ContactId);
            parameters.Add("@EventId", eventDetail.EventId);
            var res = await DbContext.Connection.QueryFirstOrDefaultAsync<EventDetail>(sqlCheck, param: parameters, transaction: DbContext.Transaction, commandType: System.Data.CommandType.Text);
            if (res!=null)
                return true;
            else
            return false;
        }

        public async Task<int> DeleteEventDetailByEventIdAndContactId(int eventId, Guid contactId)
        {
            var sql = "DELETE FROM EventDetail e WHERE e.EventId = @EventId AND e.ContactId = @ContactId";
            var parameters = new DynamicParameters();
            parameters.Add("@EventId", eventId);
            parameters.Add("@ContactId", contactId);
            var res = await DbContext.Connection.ExecuteAsync(sql, param: parameters, transaction: DbContext.Transaction, commandType: System.Data.CommandType.Text);
            return res;
        }

        public async Task<IEnumerable<EventDetail>> GetRegisterEventByEventId(int eventId)
        {
            var storeName = "Proc_EventDetail_GetListRegisterEventByEventId";
            var parameters = new DynamicParameters();
            parameters.Add("@p_EventId", eventId);
            var res = await DbContext.Connection.QueryAsync<EventDetail>(storeName, param: parameters, transaction: DbContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);
            return res;
        }
    }
}
