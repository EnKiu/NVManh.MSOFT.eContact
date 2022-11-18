using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MS.ApplicationCore.Authorization;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;
using System;
using System.Threading.Tasks;

namespace MS.eContact.Web.Controllers
{
    public class OrganizationsController : BaseController<Organization>
    {
        IUnitOfWork _unitOfWork;
        public OrganizationsController(IRepository<Organization> repository, IBaseService<Organization> baseService, IUnitOfWork unitOfWork) : base(repository, baseService)
        {
            _unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        public override async Task<IActionResult> Get()
        {
            var data = await _unitOfWork.Organizations.GetAll();
            return Ok(data);
        }

        [AllowAnonymous]
        [HttpGet("{id}/contacts")]
        public override async Task<IActionResult> Get(string id)
        {
            var data = await _unitOfWork.Organizations.GetContactsRegister(id);
            return Ok(data);
        }
    }
}
