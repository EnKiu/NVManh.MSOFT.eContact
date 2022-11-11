using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Interfaces
{
    public interface IAsyncService<TEntity>
    {
        Task<int> AddAsync(TEntity entity);
        Task<int> AddAsync(IEnumerable<TEntity> entities);
        Task<int> RemoveAsync(object key);
        Task<int> UpdateAsync(TEntity entity, object pks);
        Task<int> InstertOrUpdateAsync(TEntity entity, object pks);
    }
}
