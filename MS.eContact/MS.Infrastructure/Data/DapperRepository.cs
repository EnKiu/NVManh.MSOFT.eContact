using Dapper;
using MS.ApplicationCore.Interfaces;
using MS.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MS.Infrastructure.Data
{
    public class DapperRepository<TEntity> : IAsyncRepository<TEntity>, IRepository<TEntity> where TEntity: class
    {
        IUnitOfWork _unitOfWork = null;
        string _tableName = string.Empty;
        public DapperRepository(IUnitOfWork unitOfWork)
        {
            _tableName = typeof(TEntity).Name;
            _unitOfWork = unitOfWork;
        }
        #region Add
        public int Add(TEntity entity)
        {
            var rowAffects = 0;
            _unitOfWork.Connection.Open();
            _unitOfWork.Begin();
            try
            {
                rowAffects = _unitOfWork.Connection.Execute($"Proc_Insert{_tableName}", entity, commandType: System.Data.CommandType.StoredProcedure);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
            }
            _unitOfWork.Connection.Close();
            return rowAffects;
        }

        public int Add(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AddAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AddAsync(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region GET
        public IEnumerable<TEntity> All()
        {
            return _unitOfWork.Connection.Query<TEntity>($"SELECT * FROM {_tableName}",_unitOfWork.Transaction, commandType: System.Data.CommandType.Text);
        }

        public async Task<IEnumerable<TEntity>> AllAsync()
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
            throw new NotImplementedException();
        }

        public async Task<int> UpdateAsync(TEntity entity, object pks)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region DELETE/REMOVE
        public void Remove(object key)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveAsync(object key)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
