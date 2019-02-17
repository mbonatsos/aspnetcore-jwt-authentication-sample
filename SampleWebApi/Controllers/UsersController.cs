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
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            // map dto to entity
            var user = MapToEntity(userLoginDto);

            var accessToken = await _userService.Login(user, userLoginDto.Password);

            if (accessToken == null)
                return BadRequest();

            return Ok(new { access_token = accessToken });
        }

        private User MapToEntity(UserRegisterDto userRegisterDto)
        {
            return new User
            {
                Username = userRegisterDto.Username,
                Email = userRegisterDto.Email
            };
        }

        private User MapToEntity(UserLoginDto userRegisterDto)
        {
            return new User
            {
                Username = userRegisterDto.Username,
            };
        }
    }
}
