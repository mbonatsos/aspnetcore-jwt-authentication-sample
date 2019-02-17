using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UsersController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            var user = _mapper.Map<User>(userRegisterDto);

            user = await _userService.Register(user, userRegisterDto.Password);

            if (user == null)
                return BadRequest();

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var user = _mapper.Map<User>(userLoginDto);

            var accessToken = await _userService.Login(user, userLoginDto.Password);

            if (accessToken == null)
                return BadRequest();

            return Ok(new { access_token = accessToken });
        }
    }
}
