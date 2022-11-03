using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;

namespace MS.eContact.Web.Controllers
{
    public class ExpenditurePlansController : BaseController<ExpenditurePlan>
    {
        public ExpenditurePlansController(IRepository<ExpenditurePlan> repository, IBaseService<ExpenditurePlan> baseService) : base(repository, baseService)
        {
        }
    }
}
