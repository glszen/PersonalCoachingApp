using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalCoaching.WebApi.Jwt;
using PersonalCoaching.WebApi.Models;
using PersonalCoachingApp.Business.Operations.User;
using PersonalCoachingApp.Business.Operations.User.Dtos;

namespace PersonalCoaching.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService; //We used DI to implement a method in another class.

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        //Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var addUserDto = new AddUserDto //We converted it to DTO.
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password,
                BirthDate = request.BirthDate,
            };

            var result = await _userService.AddUser(addUserDto); //We sent it to the "Business Layer"

            if (result.IsSucceed)
                return Ok();
            else
                return BadRequest(result.Message);
        }


        [HttpPost("Login")]

        public IActionResult Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _userService.LoginUser(new LoginUserDto { Email=request.Email, Password = request.Password});

            if (!result.IsSucceed)
                return BadRequest(result.Message);

            var user = result.Data;

            var configuration = HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            var token = JwtHelper.GenerateJwtToken(new JwtDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserType = user.UserType,
                SecretKey = configuration["Jwt:SecretKey"]!,
                Issuer = configuration["Jwt:Issuer"]!,
                Audience = configuration["Jwt:Audience"]!,
                ExpireMinutes = int.Parse(configuration["Jwt:ExpireMinutes"]!)
            });

            return Ok(new LoginResponse
            {
                Message = "Login successful",
                Token = token
            });

        }

        [HttpGet("Me")]
        [Authorize] //If there is no token, no response will be returned.
        public IActionResult GetMyUser()
        {
            return Ok();
        }
    }
}
