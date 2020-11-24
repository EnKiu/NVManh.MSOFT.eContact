using System;
using System.Collections.Generic;
using System.Text;

namespace MS.ApplicationCore.Interfaces
{
    public interface IBaseService<TEntity>
    {
        IEnumerable<TEntity> All();
        IEnumerable<TEntity> GetData(string query, object parameters);
        TEntity Find(object pksFields);
        int Add(TEntity entity);
        int Add(IEnumerable<TEntity> entities);
        void Remove(object key);
        int Update(TEntity entity, object pks);
        int InstertOrUpdate(TEntity entity, object pks);
    }
}
