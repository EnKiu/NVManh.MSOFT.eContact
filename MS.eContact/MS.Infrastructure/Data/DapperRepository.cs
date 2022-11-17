using Dapper;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Exceptions;
using MS.ApplicationCore.Interfaces;
using MS.ApplicationCore.Utilities;
using MS.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static Dapper.SqlMapper;

namespace MS.Infrastructure.Data
{
    public class DapperRepository<TEntity> : IAsyncRepository<TEntity>, IRepository<TEntity> where TEntity : BaseEntity
    {
        //protected IUnitOfWork UnitOfWork = null;
        string _tableName = string.Empty;
        protected readonly MySqlDbContext DbContext;
        public DapperRepository(MySqlDbContext sqlDbContext)
        {
            _tableName = typeof(TEntity).Name;
            //UnitOfWork = unitOfWork;
            DbContext = sqlDbContext;
        }
        #region Add
        public virtual int Add(TEntity entity)
        {
            var rowAffects = 0;
            try
            {
                SetDefaultOrganization(entity);
                rowAffects = DbContext.Connection.Execute($"Proc_Insert{_tableName}", entity, transaction: DbContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw new MISAException(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
            finally
            {
                DbContext.Connection.Close();
            }
            return rowAffects;
        }

        protected void SetDefaultOrganization(TEntity entity)
        {
            var orgProp = typeof(TEntity).GetProperty("OrganizationId");
            if (orgProp != null && orgProp.GetValue(entity) == null)
            {
                orgProp.SetValue(entity, Guid.Parse(CommonFunction.GetCurrentOrganozationId()));
            }
        }

        public virtual async Task<int> AddAsync(TEntity entity)
        {
            try
            {
                SetDefaultOrganization(entity);
                var rowAffects = await DbContext.Connection.ExecuteAsync($"Proc_Insert{_tableName}", entity, transaction: DbContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);
                return rowAffects;
            }
            catch (Exception)
            {
                var rowAffects = await DbContext.AddAsync<TEntity>(entity);
                return rowAffects;
            }

        }

        public async Task<int> AddAsync(IEnumerable<TEntity> entities)
        {
            var res = 0;
            DbContext.Connection.BeginTransaction();
            foreach (var item in entities)
            {
                res += await AddAsync(item);
            }
            DbContext.Transaction.Commit();
            return res;
        }
        #endregion

        #region GET
        public virtual IEnumerable<TEntity> All()
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@p_OrganizationId", CommonFunction.GetCurrentOrganozationId());
            return DbContext.Connection.Query<TEntity>($"SELECT * FROM {_tableName}",param:parameters, transaction: DbContext.Transaction, commandType: System.Data.CommandType.Text);
        }

        public virtual async Task<IEnumerable<TEntity>> AllAsync()
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@p_OrganizationId", CommonFunction.GetCurrentOrganozationId());
            return await DbContext.Connection.QueryAsync<TEntity>($"SELECT * FROM {_tableName}", param: parameters, transaction: DbContext.Transaction, commandType: System.Data.CommandType.Text);
        }

        public TEntity Find(object pksFields)
        {

            var sql = $"SELECT * FROM {_tableName} WHERE {_tableName}Id = @Id";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", pksFields);
            parameters.Add("@p_OrganizationId", CommonFunction.GetCurrentOrganozationId());
            return DbContext.Connection.QueryFirstOrDefault<TEntity>(sql,param:parameters,transaction:DbContext.Transaction, commandType: System.Data.CommandType.Text);
        }

        public virtual async Task<TEntity> FindAsync(object pksFields)
        {
            return await DbContext.GetByIdAsync<TEntity>(pksFields.ToString());
        }

        #endregion

        #region Insert Or Update

        public int Update(TEntity entity, object pks)
        {
            var rowAffects = DbContext.Connection.Execute($"Proc_Update{_tableName}", entity, transaction: DbContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);
            return rowAffects;

        }

        public async Task<int> UpdateAsync(TEntity entity, object pks)
        {
            try
            {
                var rowAffects = await DbContext.UpdateAsync<TEntity>(entity);
                //var rowAffects = await DbContext.Connection.ExecuteAsync($"Proc_Update{_tableName}", entity, transaction: DbContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);
                return rowAffects;
            }
            catch (Exception)
            {
                var rowAffects = await DbContext.UpdateAsync<TEntity>(entity);
                return rowAffects;
            }
        }
        #endregion

        #region DELETE/REMOVE
        public async Task<int> RemoveAsync(object key)
        {
            var rowAffects = 0;
            var sql = $"DELETE FROM {_tableName} WHERE {_tableName}Id = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", key);
            rowAffects = await DbContext.Connection.ExecuteAsync(sql, parameters, transaction: DbContext.Transaction, commandType: System.Data.CommandType.Text);
            return rowAffects;
        }

        #endregion

    }
}
