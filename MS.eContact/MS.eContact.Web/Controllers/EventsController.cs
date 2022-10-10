using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;
using System.Threading.Tasks;

namespace MS.eContact.Web.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EventsController : BaseController<Event>
    {
        IEventRepository _repository;
        IConfiguration _configuration;
        readonly IFileTransfer _fileTransfer;
        private readonly IWebHostEnvironment _env;
        IEventService _service;

        public EventsController(IConfiguration configuration, IWebHostEnvironment env, IFileTransfer fileTransfer, IEventRepository repository, IEventService service) : base(repository, service)
        {
            _configuration = configuration;
            _env = env;
            _repository = repository;
            _fileTransfer = fileTransfer;
            _repository = repository;
            _service = service;
        }


        [HttpGet("contact-no-register")]
        public async Task<IActionResult> GetContactNotYetRegisterEvent(int eventId)
        {
            return Ok( await _repository.GetContactNotYetRegisterEventByEventId(eventId));
        }

        public async override Task<IActionResult> Post([FromBody] Event entity)
        {
            var res = await _service.AddAsync(entity);
            if (res > 0)
                return StatusCode(201, res);
            else
                return StatusCode(500, res);
        }

    }
}
