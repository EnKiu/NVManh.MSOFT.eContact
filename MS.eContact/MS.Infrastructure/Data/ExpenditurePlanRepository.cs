using Dapper;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;
using MS.ApplicationCore.MSEnums;
using MS.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Infrastructure.Data
{
    public class ExpenditurePlanRepository : DapperRepository<ExpenditurePlan>, IExpenditurePlanRepository
    {
        public ExpenditurePlanRepository(MySqlDbContext sqlDbContext) : base(sqlDbContext)
        {
        }

        public async Task<ExpenditurePlan> GetIncrementExpenditurePlanByEventId(object eventId)
        {
            var sql = "SELECT * FROM ExpenditurePlan e WHERE EventId = @EventId AND ExpenditurePlanType = @ExpenditureType";
            var parameters = new DynamicParameters();
            parameters.Add("@EventId", eventId);
            parameters.Add("@ExpenditureType", ExpenditurePlanType.INCREMENT_EVENT);
            var expenditurePlan = await DbContext.Connection.QueryFirstOrDefaultAsync<ExpenditurePlan>(sql, parameters, transaction:DbContext.Transaction);
            return expenditurePlan;
        }
    }
}
