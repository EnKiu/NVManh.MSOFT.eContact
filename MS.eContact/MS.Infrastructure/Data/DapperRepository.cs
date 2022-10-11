using Dapper;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Exceptions;
using MS.ApplicationCore.Interfaces;
using MS.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MS.Infrastructure.Data
{
    public class DapperRepository<TEntity> : IAsyncRepository<TEntity>, IRepository<TEntity> where TEntity : BaseEntity
    {
        IUnitOfWork _unitOfWork = null;
        string _tableName = string.Empty;
        public DapperRepository(IUnitOfWork unitOfWork)
        {
            _tableName = typeof(TEntity).Name;
            _unitOfWork = unitOfWork;
        }
        #region Add
        public virtual int Add(TEntity entity)
        {
            var rowAffects = 0;
            _unitOfWork.Connection.Open();
            _unitOfWork.Begin();
            try
            {
                rowAffects = _unitOfWork.Connection.Execute($"Proc_Insert{_tableName}", entity, _unitOfWork.Transaction, commandType: System.Data.CommandType.StoredProcedure);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw new MISAException(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
            finally
            {
                _unitOfWork.Connection.Close();
            }
            return rowAffects;
        }

        public virtual int Add(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<int> AddAsync(TEntity entity)
        {
            var rowAffects = 0;
            if (_unitOfWork.Connection.State == System.Data.ConnectionState.Connecting)
            {
                _unitOfWork.Connection.Close();
            }
            _unitOfWork.Connection.Open();
            _unitOfWork.Begin();
            try
            {
                rowAffects = await _unitOfWork.Connection.ExecuteAsync($"Proc_Insert{_tableName}", entity, _unitOfWork.Transaction, commandType: System.Data.CommandType.StoredProcedure);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
            }
            _unitOfWork.Connection.Close();
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
            return _unitOfWork.Connection.Query<TEntity>($"SELECT * FROM {_tableName}", _unitOfWork.Transaction, commandType: System.Data.CommandType.Text);
        }

        public virtual async Task<IEnumerable<TEntity>> AllAsync()
        {
            return await _unitOfWork.Connection.QueryAsync<TEntity>($"SELECT * FROM {_tableName}", _unitOfWork.Transaction, commandType: System.Data.CommandType.Text);
        }

        public TEntity Find(object pksFields)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> FindAsync(object pksFields)
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
            _unitOfWork.Connection.Open();
            _unitOfWork.Begin();
            try
            {
                rowAffects = _unitOfWork.Connection.Execute($"Proc_Update{_tableName}", entity, transaction: _unitOfWork.Transaction, commandType: System.Data.CommandType.StoredProcedure);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
            }
            _unitOfWork.Connection.Close();
            return rowAffects;
        }

        public async Task<int> UpdateAsync(TEntity entity, object pks)
        {
            var rowAffects = 0;
            _unitOfWork.Connection.Open();
            _unitOfWork.Begin();
            try
            {
                rowAffects = await _unitOfWork.Connection.ExecuteAsync($"Proc_Update{_tableName}", entity, transaction: _unitOfWork.Transaction, commandType: System.Data.CommandType.StoredProcedure);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
            }
            _unitOfWork.Connection.Close();
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
            _unitOfWork.Connection.Open();
            _unitOfWork.Begin();
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", key);
                rowAffects = await _unitOfWork.Connection.ExecuteAsync(sql, parameters, transaction: _unitOfWork.Transaction, commandType: System.Data.CommandType.Text);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
            }
            _unitOfWork.Connection.Close();
            return rowAffects;
        }
        #endregion

    }
}
