﻿using Dapper;
using MS.ApplicationCore.DTOs;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;
using MS.ApplicationCore.Utilities;
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
            parameters.Add("@p_OrganizationId", CommonFunction.GetCurrentOrganozationId());
            var data = await DbContext.Connection.QueryAsync<Expenditure>(storeName, param: parameters, transaction: DbContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);
            return data;
        }

        public async Task<IEnumerable<Expenditure>> GetExpenditures()
        {
            var storeName = "Proc_Expenditure_GetExpenditures";
            var parameters = new DynamicParameters();
            parameters.Add("@p_OrganizationId", CommonFunction.GetCurrentOrganozationId());
            var data = await DbContext.Connection.QueryAsync<Expenditure>(storeName, param: parameters, transaction: DbContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);
            return data;
        }

        public async Task<FundInfo> GetGeneralInfo()
        {
            var storeName = "Proc_Expenditure_GetGeneralInfo";
            var parameters = new DynamicParameters();
            parameters.Add("@p_OrganizationId", CommonFunction.GetCurrentOrganozationId());
            var data = await DbContext.Connection.QueryFirstOrDefaultAsync<FundInfo>(storeName, param: parameters, transaction: DbContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);
            return data;
        }

        public async Task<bool> CheckCurrentFundHasExits(Expenditure expenditure)
        {
            var sqlCommand = "SELECT e.ExpenditureId From Expenditure e WHERE e.ExpenditurePlanId = @ExpenditurePlanId AND e.ContactId = @ContactId AND e.OrganizationId = @p_OrganizationId";
            var parameters = new DynamicParameters();
            parameters.Add("@ExpenditurePlanId", expenditure.ExpenditurePlanId);
            parameters.Add("@ContactId", expenditure.ContactId);
            parameters.Add("@p_OrganizationId", CommonFunction.GetCurrentOrganozationId());
            var data = await DbContext.Connection.QueryFirstOrDefaultAsync(sqlCommand, param: parameters, transaction: DbContext.Transaction, commandType: System.Data.CommandType.Text);
            if (data!=null)
            {
                return true;
            }
            return false;
        }

        public async Task<Expenditure> GetExpenditureByPlanIdAndContactId(string planId, string contactId)
        {
            var sqlCommand = "SELECT * From Expenditure e WHERE e.ExpenditurePlanId = @ExpenditurePlanId AND e.ContactId = @ContactId";
            var parameters = new DynamicParameters();
            parameters.Add("@ExpenditurePlanId", planId.ToString());
            parameters.Add("@ContactId", contactId.ToString());
            parameters.Add("@p_OrganizationId", CommonFunction.GetCurrentOrganozationId());
            var data = await DbContext.Connection.QueryFirstOrDefaultAsync<Expenditure>(sqlCommand, param: parameters, transaction: DbContext.Transaction, commandType: System.Data.CommandType.Text);
            return data;
        }
    }
}
