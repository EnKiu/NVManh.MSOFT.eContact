using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MS.ApplicationCore.Authorization;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MS.eContact.Web.Controllers
{

    public class AlbumsController : BaseController<Album>
    {
        IAlbumService _service;
        IAlbumRepository _repository;
        public AlbumsController(IAlbumRepository repository, IAlbumService service) : base(repository, service)
        {
            _service= service;
            _repository = repository;
        }

        public async override Task<IActionResult> Post([FromForm] Album entity)
        {
            var httpRequest = HttpContext.Request;
            Album album = new Album();
            if (httpRequest.Form["album"].FirstOrDefault() != null)
            {
                album = JsonConvert.DeserializeObject<Album>(httpRequest.Form["album"].First());
            }
            var files = httpRequest.Form.Files;
            album.PictureFiles = files;
            var res = await _service.AddAsync(album);
            return Ok(res);
        }

        public async override Task<IActionResult> Get(string id)
        {
            var albumId = Guid.Parse(id);
            var pictures = await _repository.GetPicturesByAlbumId(albumId);
            return Ok(pictures);
        }

        [AllowAnonymous]
        [HttpPut("{id}/total-views")]
        public async Task<IActionResult> Put(Guid id)
        {
            var res = await _repository.UpdateTotalViewAlbum(id);
            return Ok(res);
        }
    }
}
