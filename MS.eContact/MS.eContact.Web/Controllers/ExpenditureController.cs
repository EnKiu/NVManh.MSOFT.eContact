using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;

namespace MS.eContact.Web.Controllers
{
    public class ExpenditureController : BaseController<Expenditure>
    {
        public ExpenditureController(IExpenditureRepository repository, IExpenditureService baseService) : base(repository, baseService)
        {
        }
    }
}
