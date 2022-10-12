using Dapper;
using MS.ApplicationCore.Entities;
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
    public class UserRepository : DapperRepository<AspNetUsers>, IUserRepository
    {
        public UserRepository(MySqlDbContext mySqlDbContext) : base(mySqlDbContext)
        {
        }
        public async Task<AspNetUsers> GetUserAuthenticate(string userName, string password)
        {

            var sql = "SELECT * FROM AspNetUsers WHERE (UserName = @UserName OR Email = @UserName OR PhoneNumber = @UserName) AND PasswordHash = @Password";
            var parameters = new DynamicParameters();
            parameters.Add("@UserName", userName);
            parameters.Add("@Password", password);
            var user = await DbContext.Connection.QueryFirstOrDefaultAsync<AspNetUsers>(sql, param: parameters, transaction: DbContext.Transaction);

            // get roles:
            if (user == null)
            {
                return null;
            }
            var sqlRoles = "SELECT a.Id,a.Name,a.RoleValue, b.UserId FROM AspNetRoles a INNER JOIN AspNetUserRoles b ON a.Id = b.RoleId WHERE b.UserId = @UserId";
            parameters.Add("@UserId", user.Id);
            user.Roles = await DbContext.Connection.QueryAsync<AspNetRoles>(sqlRoles, param: parameters, transaction: DbContext.Transaction);

            return user;
        }

        public async override Task<AspNetUsers> FindAsync(object id)
        {
            var sqlCommand = $"SELECT * FROM AspNetUsers WHERE Id = @UserID";
            var sqlSelectRoles = $"SELECT anr.Id,anr.RoleValue," +
                                    "anr.Name, anr.OtherName FROM AspNetRoles anr " +
                                    "LEFT JOIN AspNetUserRoles anur ON anr.Id = anur.RoleId " +
                                    "WHERE anur.UserId = @UserID ORDER BY anr.RoleValue";
            var sqlSelectEmployeeInfo = "SELECT * FROM View_Employee e WHERE e.UserId = @UserId";
            var parameters = new DynamicParameters();
            parameters.Add($"@UserID", id);
            var user = await DbContext.Connection.QueryFirstOrDefaultAsync<AspNetUsers>(sqlCommand, param: parameters, transaction: DbContext.Transaction);
            if (user != null)
            {
                user.Roles = await DbContext.Connection.QueryAsync<AspNetRoles>(sqlSelectRoles, param: parameters, transaction: DbContext.Transaction);
                //user.Employee = await UnitOfWork.Connection.QueryFirstOrDefaultAsync<Employee>(sqlSelectEmployeeInfo, param: parameters, transaction: UnitOfWork.Transaction);

            }
            return user;
        }

        public new AspNetUsers GetById(Guid id)
        {
            var sqlCommand = $"SELECT * FROM AspNetUsers WHERE Id = @UserID";
            var sqlSelectRoles = $"SELECT anr.Id,anr.RoleValue," +
                                    "anr.Name, anr.OtherName FROM AspNetRoles anr " +
                                    "LEFT JOIN AspNetUserRoles anur ON anr.Id = anur.RoleId " +
                                    "WHERE anur.UserId = @UserID ORDER BY anr.RoleValue DESC";
            var sqlSelectEmployeeInfo = "SELECT * FROM View_Employee e WHERE e.UserId = @UserId";
            var parameters = new DynamicParameters();
            parameters.Add($"@UserID", id);
            var user = DbContext.Connection.QueryFirstOrDefault<AspNetUsers>(sqlCommand, param: parameters, transaction: DbContext.Transaction);
            if (user != null)
            {
                user.Roles = DbContext.Connection.Query<AspNetRoles>(sqlSelectRoles, param: parameters, transaction: DbContext.Transaction);
                //user.Employee = UnitOfWork.Connection.QueryFirstOrDefault<Employee>(sqlSelectEmployeeInfo, param: parameters, transaction: UnitOfWork.Transaction);

            }
            return user;
        }

        public async Task<int> Register(AspNetUsers user)
        {
            //return await AddAsync<User>(user);
            return await Task.FromResult(1);
        }

        public async Task<bool> CheckEmailExist(string email)
        {

            var sql = "SELECT Email FROM AspNetUsers WHERE (Email IS NOT NULL AND Email = @Email)";
            var parameters = new DynamicParameters();
            parameters.Add("@Email", email);
            var res = await DbContext.Connection.QueryFirstOrDefaultAsync<AspNetUsers>(sql, param: parameters, transaction: DbContext.Transaction);
            if (res == null)
                return false;
            return true;
        }

        public async Task<bool> CheckPhoneNumberExist(string phone)
        {

            var sql = "SELECT PhoneNumber FROM AspNetUsers WHERE (PhoneNumber IS NOT NULL AND PhoneNumber = @PhoneNumber)";
            var parameters = new DynamicParameters();
            parameters.Add("@PhoneNumber", phone);
            var res = await DbContext.Connection.QueryFirstOrDefaultAsync<AspNetUsers>(sql, param: parameters, transaction: DbContext.Transaction);
            if (res == null)
                return false;
            return true;
        }

        public async Task<bool> CheckUserNameExist(string userName)
        {

            var sql = "SELECT UserName FROM AspNetUsers WHERE (UserName IS NOT NULL AND UserName = @UserName)";
            var parameters = new DynamicParameters();
            parameters.Add("@UserName", userName);
            var res = await DbContext.Connection.QueryFirstOrDefaultAsync<AspNetRoles>(sql, param: parameters, transaction: DbContext.Transaction);
            if (res == null)
                return false;
            return true;
        }

        public async Task<IList<string>> GetRolesAsync(AspNetUsers user)
        {
            var sql = "Proc_GetRolesByUserName";
            var parameters = new DynamicParameters();
            parameters.Add("@p_UserName", user.UserName);
            var data = await DbContext.Connection.QueryAsync<string>(sql, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
            return data.ToList();
        }

        public async Task<AspNetUsers> FindByNameAsync(string userName)
        {

            var sql = "SELECT * FROM AspNetUsers WHERE (UserName = @UserName OR Email = @UserName OR PhoneNumber = @UserName)";
            var parameters = new DynamicParameters();
            parameters.Add("@UserName", userName);
            var data = await DbContext.Connection.QueryFirstOrDefaultAsync<AspNetUsers>(sql, param: parameters, transaction: DbContext.Transaction);
            return data;
        }

        public async Task<int> AddToken(UserTokenConfirm userTokenConfirm)
        {
            //using (var connection = new MySqlConnection(ConnectionString))
            //{
            //    connection.Open();
            //    var sqlAdd = BuildAddQuery<UserTokenConfirm>(userTokenConfirm);
            //    var res = await connection.ExecuteAsync(sqlAdd, param: userTokenConfirm);
            //    return res;
            //}
            return await Task.FromResult(1);
        }

        public async Task<bool> ConfirmUserToken(UserTokenConfirm userTokenConfirm)
        {
            try
            {
                var sql = "SELECT * FROM UserTokenConfirm WHERE UserName = @UserName";
                var parameters = new DynamicParameters();
                parameters.Add("@UserName", userTokenConfirm.UserName);
                var useToken = await DbContext.Connection.QueryFirstOrDefaultAsync<UserTokenConfirm>(sql, param: parameters, transaction: DbContext.Transaction);
                if (useToken.TokenCode == userTokenConfirm.TokenCode)
                {

                    // Cập nhật tình trạng xác nhận:
                    var sqlUpdateConfirm = "UPDATE AspNetUsers anu SET anu.EmailConfirmed = 1 WHERE anu.Id = @UserId;" +
                        "DELETE FROM UserTokenConfirm WHERE UserId = @UserId";
                    parameters.Add("@UserId", useToken.UserId);
                    var res = await DbContext.Connection.ExecuteAsync(sqlUpdateConfirm, param: parameters, transaction: DbContext.Transaction);
                    if (res > 0)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<IEnumerable<AspNetRoles>> GetRoles()
        {

            var sql = "SELECT * FROM AspNetRoles";
            var data = await DbContext.Connection.QueryAsync<AspNetRoles>(sql);
            return data;
        }

        public async Task<int> ConfirmMultiUser(IEnumerable<AspNetUsers> users)
        {
            var res = 0;
            if (users != null)
            {
                foreach (var user in users)
                {
                    var sqlUpdateConfirm = "UPDATE AspNetUsers anu SET anu.EmailConfirmed = 1 WHERE anu.Id = @UserId;";
                    var sqlDeleteConfirm = "DELETE FROM UserTokenConfirm WHERE UserId = @UserId";

                    var parameters = new DynamicParameters();
                    parameters.Add("@UserId", user.Id);

                    // Cập nhật tình trạng
                    res += await DbContext.Connection.ExecuteAsync(sqlUpdateConfirm, param: parameters, transaction: DbContext.Transaction);

                    // Xóa Token (nếu có):
                    await DbContext.Connection.ExecuteAsync(sqlDeleteConfirm, param: parameters, transaction: DbContext.Transaction);
                }
            }
            return res;
        }
    }
}
