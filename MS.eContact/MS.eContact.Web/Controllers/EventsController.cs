using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MS.ApplicationCore.Authorization;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Entities.Auth;
using MS.ApplicationCore.Interfaces;
using MS.ApplicationCore.MSEnums;
using System.Linq;
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

        [Authorize(MSRole.Member)]
        [HttpGet("event-left-time")]
        public async Task<IActionResult> GetEventLeftTime()
        {
            return Ok(await _repository.GetEventLeftTime());
        }


        [AllowAnonymous]
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

        /// <summary>
        /// Hủy đăng ký tham gia sự kiện
        /// </summary>
        /// <param name="eventId">Id sự kiện</param>
        /// <returns></returns>
        [HttpDelete("register")]
        public async Task<IActionResult> CanCelRegister(int eventId)
        {
            var userId = HttpContext.User?.Claims?.First(x => x.Type == "id").Value;
            var res = await _repository.DeleteEventDetailByEventIdAndUserId(eventId, userId);
            if (res > 0)
                return StatusCode(201, res);
            else
                return StatusCode(500, res);
        }
    }
}
