using AutoMapper;
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
        readonly IRepository<TEntity> _repository;
        protected Dictionary<string, List<string>> Errors = new();
        protected IUnitOfWork UnitOfWork;
        protected readonly IMapper Mapper;
        public BaseService(IRepository<TEntity> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            UnitOfWork = unitOfWork;
            Mapper = mapper;
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

        public virtual async Task<int> AddAsync(TEntity entity)
        {
            ValidateObject(entity);
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

        public virtual async Task RemoveAsync(object key)
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
        protected virtual void ValidateObjectCustom(TEntity entity)
        {

        }

        /// <summary>
        /// Hàm thực hiện thêm mới thông tin lỗi vào Errors:
        /// </summary>
        /// <param name="key">Khóa để xác định lỗi</param>
        /// <param name="errorMsg">Nội dung thông báo lỗi</param>
        /// CreatedBy: NVMANH (17/10/2022)
        protected void AddErrors(string key, string errorMsg)
        {
            // Kiểm tra xem key hiện tại đã có errors chưa?
            // Nếu có thì thực hiện add thêm, nếu chưa thì tạo mới:
            Errors.TryGetValue(key, out var error);
            if (error != null)
                error.Add(errorMsg);
            else
            {
                error = new List<string>() { errorMsg };
                Errors.Add(key, error);
            }

        }
    }
}
