using AutoMapper;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Services
{
    public class ExpenditurePlanService : BaseService<ExpenditurePlan>, IExpenditurePlanService
    {
        public ExpenditurePlanService(IRepository<ExpenditurePlan> repository, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, unitOfWork, mapper)
        {
        }

        protected override async Task BeforeSave(ExpenditurePlan entity)
        {
            if (entity.StartDate != null)
            {
                entity.StartDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)entity.StartDate, "SE Asia Standard Time");
            }
            if (entity.EndDate != null)
                entity.EndDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)entity.EndDate, "SE Asia Standard Time");
        }
    }
}
