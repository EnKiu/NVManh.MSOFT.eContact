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
    public class EventDetailRepository:DapperRepository<EventDetail>, IEventDetailRepository
    {
        IUnitOfWork _unitOfWork = null;
        string _tableName = string.Empty;
        public EventDetailRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _tableName = typeof(Event).Name;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CheckRegisted(EventDetail eventDetail)
        {
            var sqlCheck = "SELECT * FROM EventDetail e WHERE e.ContactId = @ContactId AND e.EventId = @EventId";
            var parameters = new DynamicParameters();
            parameters.Add("@ContactId", eventDetail.ContactId);
            parameters.Add("@EventId", eventDetail.EventId);
            var res = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<EventDetail>(sqlCheck, param: parameters, transaction: _unitOfWork.Transaction, commandType: System.Data.CommandType.Text);
            if (res!=null)
                return true;
            else
            return false;
        }
    }
}
