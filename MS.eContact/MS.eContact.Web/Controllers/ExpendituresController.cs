using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;

namespace MS.eContact.Web.Controllers
{
    public class ExpendituresController : BaseController<Expenditure>
    {
        public ExpendituresController(IExpenditureRepository repository, IExpenditureService baseService) : base(repository, baseService)
        {
        }
    }
}
