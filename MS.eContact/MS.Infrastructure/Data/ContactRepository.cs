using Dapper;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;
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
            return await DbContext.Connection.QueryAsync<Contact>($"SELECT * FROM Contact c ORDER BY c.SortOrder", transaction: DbContext.Transaction, commandType: System.Data.CommandType.Text);
        }

        public override IEnumerable<Contact> All()
        {
            return  DbContext.Connection.Query<Contact>($"SELECT * FROM Contact c ORDER BY c.SortOrder", transaction: DbContext.Transaction, commandType: System.Data.CommandType.Text);
        }
    }
}
