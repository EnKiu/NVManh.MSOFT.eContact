using Microsoft.AspNetCore.Mvc;
using MS.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MS.eContact.Web.Controllers
{
    [Route("/api/v1/[controller]")]
    [ApiController]
    public class BaseController<TEntity> : ControllerBase where TEntity: class
    {
        IRepository<TEntity> _repository;
        public BaseController(IRepository<TEntity> repository)
        {
            _repository = repository;
        }
        // GET: api/<BaseController>
        [HttpGet]
        public IEnumerable<TEntity> Get()
        {
            return _repository.All();
        }

        // GET api/<BaseController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<BaseController>
        [HttpPost]
        public int Post([FromBody] TEntity entity)
        {
           return _repository.Add(entity);
        }

        // PUT api/<BaseController>/5
        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put(string id)
        {
           
            return await Task.FromResult(Ok());
        }

        // DELETE api/<BaseController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
