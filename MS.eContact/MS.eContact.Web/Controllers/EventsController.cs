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
        public EventsController(IConfiguration configuration, IWebHostEnvironment env, IFileTransfer fileTransfer, IEventRepository repository) : base(repository)
        {
            _configuration = configuration;
            _env = env;
            _repository = repository;
            _fileTransfer = fileTransfer;
            _repository = repository;
        }

        [HttpGet("contact-no-register")]
        public async Task<IActionResult> GetContactNotYetRegisterEvent(int eventId)
        {
            return Ok( await _repository.GetContactNotYetRegisterEventByEventId(eventId));
        }
    }
}
