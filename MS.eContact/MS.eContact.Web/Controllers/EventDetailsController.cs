using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MS.ApplicationCore.Authorization;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Exceptions;
using MS.ApplicationCore.Interfaces;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
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

        [AllowAnonymous]
        [HttpGet("registers")]
        public async Task<IActionResult> GetListRegisterEvent(int eventId)
        {
            var data = await _repository.GetRegisterEventByEventId(eventId);
            return Ok(data);
        }

        [AllowAnonymous]
        public override Task<IActionResult> Post([FromBody] EventDetail entity)
        {
            return base.Post(entity);
        }

        /// <summary>
        /// Cập nhật thông tin người đăng ký sự kiện
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<IActionResult> Put(string id, [FromBody] EventDetail entity)
        {
            var res = await _service.UpdateAsync(entity, id);
            if (res > 0)
                return Ok(res);
            else
                throw new MISAException(HttpStatusCode.InternalServerError, "Không thể cập nhập dữ liệu, vui lòng liên hệ Quản trị viên để được trợ giúp.");
        }
        //public override async Task<IActionResult> Delete(object id)
        //{
        //    var httpRequest = HttpContext.Request;
        //    var eventDetail = JsonConvert.DeserializeObject<EventDetail>(httpRequest.Form["EventDetail"].First());
        //    return Ok(await _repository.DeleteEventDetailByEventIdAndContactId(eventDetail.EventId, eventDetail.ContactId));
        //}
    }
}
