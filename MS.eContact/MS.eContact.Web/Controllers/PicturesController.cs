using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;
using System;
using System.Threading.Tasks;

namespace MS.eContact.Web.Controllers
{
    public class PicturesController : BaseController<Picture>
    {
        IPictureRepository _pictureRepository;
        public PicturesController(IPictureRepository repository, IPictureService service) : base(repository, service)
        {
            _pictureRepository= repository;
        }

        [HttpPut("{id}/total-views")]
        public async Task<IActionResult> Put(Guid id)
        {
            var res = await _pictureRepository.UpdateTotalViewPicture(id);
            return Ok(res);
        }
    }
}
