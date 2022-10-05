using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;
using System;
using System.Threading.Tasks;

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

        [HttpGet("registers")]
        public async Task<IActionResult> GetListRegisterEvent(int eventId)
        {
            var data = await _repository.GetRegisterEventByEventId(eventId);
            return Ok(data);
        }
    }
}
