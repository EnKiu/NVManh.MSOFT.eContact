using MS.ApplicationCore.DTOs;
using MS.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Interfaces
{
    public interface IExpenditureRepository:IRepository<Expenditure>
    {
        Task<IEnumerable<Expenditure>> GetRevenues();
        Task<IEnumerable<Expenditure>> GetExpenditures();
        Task<FundInfo> GetGeneralInfo();
    }
}
