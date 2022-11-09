using MS.ApplicationCore.DTOs;
using MS.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Interfaces
{
    public interface IExpenditurePlanRepository:IRepository<ExpenditurePlan>
    {
        /// <summary>
        /// Lấy kế hoạch thu chi theo sự kiện.
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        Task<ExpenditurePlan> GetIncrementExpenditurePlanByEventId(object eventId);
        Task<IEnumerable<ExpenditurePlanResponse>> GetExpenditurePlans(int? type=null);
    }
}
