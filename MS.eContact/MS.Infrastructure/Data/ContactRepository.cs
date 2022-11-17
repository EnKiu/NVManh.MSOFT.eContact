using Dapper;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;
using MS.ApplicationCore.Utilities;
using MS.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace MS.Infrastructure.Data
{
    public class ContactRepository:DapperRepository<Contact>,IContactRepository
    {
        string _tableName = string.Empty;
        public ContactRepository(MySqlDbContext dbContext):base(dbContext)
        {
            _tableName = typeof(Contact).Name;
        }

        public async override Task<IEnumerable<Contact>> AllAsync()
        {
            var sql = "SELECT * FROM Contact c WHERE c.OrganizationId = @p_OrganizationId ORDER BY c.SortOrder";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@p_OrganizationId", CommonFunction.GetCurrentOrganozationId());
            return await DbContext.Connection.QueryAsync<Contact>(sql, param:parameters, transaction: DbContext.Transaction, commandType: System.Data.CommandType.Text);
        }

        public override IEnumerable<Contact> All()
        {
            var sql = "SELECT * FROM Contact c WHERE c.OrganizationId = @p_OrganizationId ORDER BY c.SortOrder";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@p_OrganizationId", CommonFunction.GetCurrentOrganozationId());
            return  DbContext.Connection.Query<Contact>(sql, param: parameters, transaction: DbContext.Transaction, commandType: System.Data.CommandType.Text);
        }
    }
}
