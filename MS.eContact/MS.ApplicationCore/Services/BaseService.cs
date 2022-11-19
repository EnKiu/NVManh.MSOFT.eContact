using AutoMapper;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Exceptions;
using MS.ApplicationCore.Interfaces;
using MS.ApplicationCore.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Services
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity: BaseEntity
    {
        readonly IRepository<TEntity> _repository;
        protected Dictionary<string, List<string>> Errors = new();
        public IUnitOfWork UnitOfWork;
        protected readonly IMapper Mapper;
        public BaseService(IRepository<TEntity> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }

        public virtual async Task<int> AddAsync(TEntity entity)
        {
            entity.EntityState = MSEnums.EntityState.ADD;
            await ValidateObject(entity);
            await ProcessEntityBeforeSave(entity);
            if (Errors.Count == 0)
            {
                var res = await _repository.AddAsync(entity);
                await AfterSave(entity);
                return res;
            }
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

        public virtual async Task<int> RemoveAsync(object key)
        {
            var res = await _repository.RemoveAsync(key);
            await AfterDeleted();
            return await _repository.RemoveAsync(key);
        }

        public virtual async Task<int> UpdateAsync(TEntity entity, object pks)
        {
            entity.EntityState = MSEnums.EntityState.UPDATE;
            await ValidateObject(entity);
            await ProcessEntityBeforeSave(entity);
            if (Errors.Count == 0)
            {
                var res = await _repository.UpdateAsync(entity, pks);
                await AfterSave(entity);
                return res;
            }
            else
                throw new MISAException(System.Net.HttpStatusCode.BadRequest, Errors);
        }

        private async Task ProcessEntityBeforeSave(TEntity entity)
        {
            var timeNow = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "SE Asia Standard Time");
            if (entity.EntityState == MSEnums.EntityState.ADD)
            {
                var createdDate = typeof(TEntity).GetProperty("CreatedDate");
                var createdBy = typeof(TEntity).GetProperty("CreatedBy");
                if (createdDate != null)
                {
                    createdDate.SetValue(entity, timeNow);
                }
                if (createdBy != null)
                {
                    createdBy.SetValue(entity, CommonFunction.UserId);
                }
            }
            else
            {
                var mdifiedDate = typeof(TEntity).GetProperty("ModifiedDate");
                var modifiedBy = typeof(TEntity).GetProperty("ModifiedBy");
                if (mdifiedDate != null)
                {
                    mdifiedDate.SetValue(entity, timeNow);
                }
                if (modifiedBy != null)
                {
                    modifiedBy.SetValue(entity, CommonFunction.UserId);
                }
            }
            await BeforeSave(entity);
        }
        protected virtual async Task BeforeSave(TEntity entity)
        {
            
        }

        protected virtual async Task AfterSave(TEntity entity)
        {

        }

        protected virtual async Task ValidateObject(TEntity entity)
        {

        }
        protected virtual async Task ValidateObjectCustom(TEntity entity)
        {

        }

        protected virtual async Task AfterDeleted()
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
