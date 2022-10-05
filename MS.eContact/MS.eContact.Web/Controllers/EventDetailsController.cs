using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;

namespace MS.eContact.Web.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EventDetailsController : BaseController<EventDetail>
    {
        IEventDetailService _service;
        IEventDetailRepository _repository;
        public EventDetailsController(IEventDetailService service, IEventDetailRepository repository) : base(repository, service)
        {
            _service = service;
            _repository = repository;
        }
    }
}
