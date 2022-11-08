﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MS.ApplicationCore.Authorization;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;
using System.Threading.Tasks;

namespace MS.eContact.Web.Controllers
{
    public class ExpenditurePlansController : BaseController<ExpenditurePlan>
    {
        IDictionaryEnumService _dictionaryEnumService;
        public ExpenditurePlansController(IRepository<ExpenditurePlan> repository, IBaseService<ExpenditurePlan> baseService, IDictionaryEnumService dictionaryEnumService) : base(repository, baseService)
        {
            _dictionaryEnumService = dictionaryEnumService;
        }

        [AllowAnonymous]
        [HttpGet("plan-type")]
        public IActionResult GetExpenditurePlanTypeList()
        {
            return Ok(_dictionaryEnumService.GetExpenditurePlanType());
        }
    }
}
