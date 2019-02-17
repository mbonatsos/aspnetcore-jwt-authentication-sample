using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleWebApi.Dtos;
using SampleWebApi.Models;
using SampleWebApi.Services;
using System.Threading.Tasks;

namespace SampleWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            // map dto to entity
            var user = MapToEntity(userRegisterDto);

            user = await _userService.Register(user, userRegisterDto.Password);

            if (user == null)
                return BadRequest();

            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login()
        {
            return Ok();
        }

        private User MapToEntity(UserRegisterDto userRegisterDto)
        {
            return new User
            {
                Username = userRegisterDto.Username,
                Email = userRegisterDto.Email
            };
        }
    }
}
