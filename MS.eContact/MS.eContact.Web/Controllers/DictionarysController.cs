using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MS.ApplicationCore.Authorization;
using MS.ApplicationCore.Interfaces;

namespace MS.eContact.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictionarysController : ControllerBase
    {
        IDictionaryEnumService _dictionaryEnumService;
        public DictionarysController(IDictionaryEnumService dictionaryEnumService)
        {
            _dictionaryEnumService = dictionaryEnumService;
        }

        [AllowAnonymous]
        [HttpGet("plan-type")]
        public IActionResult GetExpenditurePlanTypeList()
        {
            var res = _dictionaryEnumService.GetExpenditurePlanType();
            return Ok(res);
        }

        [AllowAnonymous]
        [HttpGet("gender")]
        public IActionResult GetGenders()
        {
            return Ok(_dictionaryEnumService.GetGenders());
        }
    }
}
