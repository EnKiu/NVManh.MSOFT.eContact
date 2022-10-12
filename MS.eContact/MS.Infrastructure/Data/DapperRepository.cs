using Dapper;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Exceptions;
using MS.ApplicationCore.Interfaces;
using MS.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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
            DbContext.Connection.Open();
            var transaction = DbContext.Connection.BeginTransaction();
            try
            {
                rowAffects = DbContext.Connection.Execute($"Proc_Insert{_tableName}", entity, transaction: DbContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new MISAException(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
            finally
            {
                DbContext.Connection.Close();
            }
            return rowAffects;
        }

        public virtual int Add(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<int> AddAsync(TEntity entity)
        {
            var rowAffects = await DbContext.Connection.ExecuteAsync($"Proc_Insert{_tableName}", entity, transaction: DbContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);

            return rowAffects;
        }

        public async Task<int> AddAsync(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region GET
        public virtual IEnumerable<TEntity> All()
        {
            return DbContext.Connection.Query<TEntity>($"SELECT * FROM {_tableName}", transaction: DbContext.Transaction, commandType: System.Data.CommandType.Text);
        }

        public virtual async Task<IEnumerable<TEntity>> AllAsync()
        {
            return await DbContext.Connection.QueryAsync<TEntity>($"SELECT * FROM {_tableName}", transaction: DbContext.Transaction, commandType: System.Data.CommandType.Text);
        }

        public TEntity Find(object pksFields)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<TEntity> FindAsync(object pksFields)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetData(string qry, object parameters)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TEntity>> GetDataAsync(string qry, object parameters)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Insert Or Update
        public int InstertOrUpdate(TEntity entity, object pks)
        {
            throw new NotImplementedException();
        }

        public async Task<int> InstertOrUpdateAsync(TEntity entity, object pks)
        {
            throw new NotImplementedException();
        }

        public int Update(TEntity entity, object pks)
        {
            var rowAffects = 0;
            DbContext.Connection.Open();
            var transaction = DbContext.Connection.BeginTransaction();
            try
            {
                rowAffects = DbContext.Connection.Execute($"Proc_Update{_tableName}", entity, transaction: transaction, commandType: System.Data.CommandType.StoredProcedure);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
            DbContext.Connection.Close();
            return rowAffects;
        }

        public async Task<int> UpdateAsync(TEntity entity, object pks)
        {
            var rowAffects = 0;
            DbContext.Connection.Open();
            var transaction = DbContext.Connection.BeginTransaction();
            try
            {
                rowAffects = await DbContext.Connection.ExecuteAsync($"Proc_Update{_tableName}", entity, transaction: transaction, commandType: System.Data.CommandType.StoredProcedure);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
            DbContext.Connection.Close();
            return rowAffects;
        }
        #endregion

        #region DELETE/REMOVE
        public void Remove(object key)
        {
            throw new NotImplementedException();
        }

        public async Task<int> RemoveAsync(object key)
        {
            var rowAffects = 0;
            var sql = $"DELETE FROM {_tableName} WHERE {_tableName}Id = @Id";
            DbContext.Connection.Open();
            var transaction = DbContext.Connection.BeginTransaction();
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", key);
                rowAffects = await DbContext.Connection.ExecuteAsync(sql, parameters, transaction: transaction, commandType: System.Data.CommandType.Text);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
            DbContext.Connection.Close();
            return rowAffects;
        }

        #endregion

    }
}
