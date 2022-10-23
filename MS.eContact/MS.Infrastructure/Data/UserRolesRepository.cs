using Dapper;
using MS.ApplicationCore.Entities.Auth;
using MS.ApplicationCore.Interfaces;
using MS.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Infrastructure.Data
{
    public class UserRolesRepository : DapperRepository<UserRole>, IUserRolesRepository
    {
        public UserRolesRepository(MySqlDbContext sqlDbContext) : base(sqlDbContext)
        {
        }

        public async Task<Guid> GetUserRoleIdByRoleValue(int roleValue)
        {
            var sql = "SELECT RoleId FROM Role WHERE RoleValue = @RoleValue";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@RoleValue", roleValue);
            var userRoleId = await DbContext.Connection.QueryFirstOrDefaultAsync<Guid>(sql, param: parameters, transaction: DbContext.Transaction);
            return userRoleId;
        }
    }
}
