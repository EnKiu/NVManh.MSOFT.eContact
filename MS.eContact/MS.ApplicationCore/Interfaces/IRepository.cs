using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Interfaces
{
    public interface IRepository<TEntity>:IAsyncRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> All();
        int Update(TEntity entity, object pks);
    }
}
