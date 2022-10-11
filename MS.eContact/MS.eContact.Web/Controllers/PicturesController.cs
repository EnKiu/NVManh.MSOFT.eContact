using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;

namespace MS.eContact.Web.Controllers
{
    public class PicturesController : BaseController<Picture>
    {
        public PicturesController(IPictureRepository repository, IPictureService service) : base(repository, service)
        {
        }
    }
}
