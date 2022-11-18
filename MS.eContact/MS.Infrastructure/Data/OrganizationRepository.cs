using Dapper;
using MS.ApplicationCore.DTOs;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Entities.Auth;
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
    public class OrganizationRepository : DapperRepository<Organization>,IOrganizationRepository
    {
        public OrganizationRepository(MySqlDbContext sqlDbContext) : base(sqlDbContext)
        {
        }

        public async Task<IEnumerable<OrganizationRegister>> GetAll()
        {
            var storeName = "Proc_GetOrganizationRegister";
            var data = await DbContext.Connection.QueryAsync<OrganizationRegister>(storeName,transaction:DbContext.Transaction,commandType:System.Data.CommandType.StoredProcedure);
            return data;
        }

        public async Task<IEnumerable<ContactRegister>> GetContactsRegister(string organizationId)
        {
            var sql = "SELECT c.ContactId,c.FullName,OrganizationId FROM Contact c LEFT JOIN User u ON c.ContactId = u.ContactId " +
                "WHERE u.ContactId IS NULL AND c.OrganizationId = @OrganizationId";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("OrganizationId", organizationId);
            var data = await DbContext.Connection.QueryAsync<ContactRegister>(sql, parameters, transaction: DbContext.Transaction);
            return data;
        }
    }
}
