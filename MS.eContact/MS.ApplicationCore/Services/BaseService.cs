using MS.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.ApplicationCore.Services
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity:class
    {
        IRepository<TEntity> _repository;
        public BaseService(IRepository<TEntity> repository)
        {
            _repository = repository;
        }
        public int Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public int Add(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> All()
        {
            return _repository.All();
        }

        public TEntity Find(object pksFields)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetData(string query, object parameters)
        {
            throw new NotImplementedException();
        }

        public int InstertOrUpdate(TEntity entity, object pks)
        {
            throw new NotImplementedException();
        }

        public void Remove(object key)
        {
            throw new NotImplementedException();
        }

        public int Update(TEntity entity, object pks)
        {
            throw new NotImplementedException();
        }
    }
}
