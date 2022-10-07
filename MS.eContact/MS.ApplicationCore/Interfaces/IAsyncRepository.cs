﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Interfaces
{
    public interface IAsyncRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> AllAsync();
        Task<IEnumerable<TEntity>> GetDataAsync(string qry, object parameters);
        Task<TEntity> FindAsync(object pksFields);
        Task<int> AddAsync(TEntity entity);
        Task<int> AddAsync(IEnumerable<TEntity> entities);
        Task<int> RemoveAsync(object key);
        Task<int> UpdateAsync(TEntity entity, object pks);
        Task<int> InstertOrUpdateAsync(TEntity entity, object pks);
    }
}
