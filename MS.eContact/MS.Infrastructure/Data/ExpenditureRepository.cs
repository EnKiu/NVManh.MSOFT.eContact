using Dapper;
using MS.ApplicationCore.DTOs;
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
    public class ExpenditureRepository : DapperRepository<Expenditure>,IExpenditureRepository
    {
        public ExpenditureRepository(MySqlDbContext sqlDbContext) : base(sqlDbContext)
        {
        }

        public async Task<IEnumerable<Expenditure>> GetRevenues()
        {
            var storeName = "Proc_Expenditure_GetRevenues";
            var parameters = new DynamicParameters();
            var data = await DbContext.Connection.QueryAsync<Expenditure>(storeName, param: parameters, transaction: DbContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);
            return data;
        }

        public async Task<IEnumerable<Expenditure>> GetExpenditures()
        {
            var storeName = "Proc_Expenditure_GetExpenditures";
            var parameters = new DynamicParameters();
            var data = await DbContext.Connection.QueryAsync<Expenditure>(storeName, param: parameters, transaction: DbContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);
            return data;
        }

        public async Task<FundInfo> GetGeneralInfo()
        {
            var storeName = "Proc_Expenditure_GetGeneralInfo";
            var parameters = new DynamicParameters();
            var data = await DbContext.Connection.QueryFirstOrDefaultAsync<FundInfo>(storeName, param: parameters, transaction: DbContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);
            return data;
        }
    }
}
