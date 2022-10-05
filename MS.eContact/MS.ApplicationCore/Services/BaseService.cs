using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Exceptions;
using MS.ApplicationCore.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Services
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity: BaseEntity
    {
        IRepository<TEntity> _repository;
        protected Dictionary<string, List<string>> Errors = new Dictionary<string,List<string>>();
        public BaseService(IRepository<TEntity> repository)
        {
            _repository = repository;
        }
        public int Add(TEntity entity)
        {
            ValidateObject(entity);
            if (Errors.Count == 0)
                return _repository.Add(entity);
            else
                throw new MISAException(System.Net.HttpStatusCode.BadRequest,Errors);
        }

        public int Add(IEnumerable<TEntity> entities)
        {
            if (Errors.Count == 0)
                return _repository.Add(entities);
            else
                throw new MISAException(System.Net.HttpStatusCode.BadRequest, Errors);
        }

        public async Task<int> AddAsync(TEntity entity)
        {
            if (Errors.Count == 0)
                return await _repository.AddAsync(entity);
            else
                throw new MISAException(System.Net.HttpStatusCode.BadRequest, Errors);
        }

        public async Task<int> AddAsync(IEnumerable<TEntity> entities)
        {
            if (Errors.Count == 0)
                return await _repository.AddAsync(entities);
            else
                throw new MISAException(System.Net.HttpStatusCode.BadRequest, Errors);
        }

        public IEnumerable<TEntity> All()
        {
            return _repository.All();
        }

        public TEntity Find(object pksFields)
        {
            return _repository.Find(pksFields);
        }

        public IEnumerable<TEntity> GetData(string query, object parameters)
        {
            return _repository.GetData(query, parameters);
        }

        public int InstertOrUpdate(TEntity entity, object pks)
        {
            if (Errors.Count == 0)
                return _repository.InstertOrUpdate(entity, pks);
            else
                throw new MISAException(System.Net.HttpStatusCode.BadRequest, Errors);
           
        }

        public async Task<int> InstertOrUpdateAsync(TEntity entity, object pks)
        {
            if (Errors.Count == 0)
                return await _repository.InstertOrUpdateAsync(entity, pks);
            else
                throw new MISAException(System.Net.HttpStatusCode.BadRequest, Errors);
            
        }

        public void Remove(object key)
        {
            _repository.Remove(key);
        }

        public async Task RemoveAsync(object key)
        {
            await _repository.RemoveAsync(key);
        }

        public int Update(TEntity entity, object pks)
        {
            if (Errors.Count == 0)
                return _repository.Update(entity, pks);
            else
                throw new MISAException(System.Net.HttpStatusCode.BadRequest, Errors);
        }

        public async Task<int> UpdateAsync(TEntity entity, object pks)
        {
            if (Errors.Count == 0)
                return await _repository.UpdateAsync(entity, pks);
            else
                throw new MISAException(System.Net.HttpStatusCode.BadRequest, Errors);
        }

        protected virtual void ValidateObject(TEntity entity)
        {

        }
    }
}
