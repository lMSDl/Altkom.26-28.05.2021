using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _service.Read();

            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult Get(int id)
        {
            var result = _service.Read(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post(User user)
        {
            var id = _service.Create(user);
            return CreatedAtRoute("GetUser", new { id = id }, id);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, User user)
        {
            if (_service.Read(id) == null)
                return NotFound();

            _service.Update(id, user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_service.Read(id) == null)
                return NotFound();

            _service.Delete(id);
            return NoContent();
        }
    }
}
