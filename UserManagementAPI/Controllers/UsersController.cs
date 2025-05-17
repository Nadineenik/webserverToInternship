using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;
using UserManagementAPI.Services;
using System;
using System.Collections.Generic;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        public UsersController(IUserService service) => _service = service;
        [HttpPost]
        public ActionResult<User> Create([FromBody] User user)
        {
            // Admin only: assume auth mocked via header
            var createdBy = Request.Headers["X-User"].ToString();
            if (string.IsNullOrEmpty(createdBy)) return Unauthorized();
            user.Admin = user.Admin;
            return Ok(_service.Create(user, createdBy));
        }

        [HttpPut("profile/{login}")]
        public ActionResult<User> UpdateProfile(string login, [FromBody] User updated)
            => Ok(_service.UpdateProfile(login, updated.Name, updated.Gender, updated.Birthday, Request.Headers["X-User"]));

        [HttpPut("password/{login}")]
        public ActionResult<User> UpdatePassword(string login, [FromBody] string newPassword)
            => Ok(_service.UpdatePassword(login, newPassword, Request.Headers["X-User"]));

        [HttpPut("login/{oldLogin}")]
        public ActionResult<User> UpdateLogin(string oldLogin, [FromBody] string newLogin)
            => Ok(_service.UpdateLogin(oldLogin, newLogin, Request.Headers["X-User"]));

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllActive() => Ok(_service.GetAllActive());

        [HttpGet("{login}")]
        public ActionResult<User> GetByLogin(string login) => Ok(_service.GetByLogin(login));

        [HttpPost("auth")]
        public ActionResult<User> Authenticate([FromBody] LoginRequest req)
            => Ok(_service.Authenticate(req.Login, req.Password));

        [HttpGet("olderThan/{age}")]
        public ActionResult<IEnumerable<User>> GetOlderThan(int age) => Ok(_service.GetOlderThan(age));

        [HttpDelete("{login}")]
        public ActionResult Revoke(string login, [FromQuery] bool hard = false)
        {
            _service.Revoke(login, hard, Request.Headers["X-User"]);
            return NoContent();
        }

        [HttpPost("restore/{login}")]
        public ActionResult Restore(string login)
        {
            _service.Restore(login, Request.Headers["X-User"]);
            return NoContent();
        }
    }

    public class LoginRequest { public string Login { get; set; } public string Password { get; set; } }
}