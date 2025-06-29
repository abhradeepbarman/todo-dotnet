using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoApp.Dto;
using TodoApp.Models;
using TodoApp.Services.Interfaces;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IUserService userService, IPasswordService passwordService, IJwtService jwtService) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IPasswordService _passwordService = passwordService;
        private readonly IJwtService _jwtService = jwtService;

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponseDto>> UserRegister([FromBody] RegisterRequestDto registerRequestDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userService.GetUserByEmail(registerRequestDto.Email);
            if (existingUser != null) { 
                return BadRequest("User already exists");
            }

            var hashedPassword = _passwordService.HashedPassword(registerRequestDto.Password);
            var user = new User
            {
                Name = registerRequestDto.Name,
                Email = registerRequestDto.Email,
                Password = hashedPassword,
            };

            var newUser = await _userService.CreateUser(user);
            var tokens = _jwtService.GenerateToken(newUser.Id);
            await _userService.SaveToken(newUser.Id, token: tokens.RefreshToken);

            return new RegisterResponseDto
            {
                Id = newUser.Id,
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> UserLogin(LoginRequestDto loginRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.GetUserByEmail(loginRequestDto.Email);
            if(user == null)
            {
                return BadRequest("User does not exist");
            }

            var isPasswordMatch = _passwordService.VerifyPassowrd(loginRequestDto.Password, user.Password);

            if(!isPasswordMatch)
            {
                return BadRequest("Password does not match");
            }

            var tokens = _jwtService.GenerateToken(user.Id);
            await _userService.SaveToken(user.Id, token: tokens.RefreshToken);

            return new LoginResponseDto
            {
                Id = user.Id,
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
            };
        }

        [HttpPut("refresh-token")]
        public async Task<ActionResult<RefreshTokenResponseDto>> RefreshToken(string token)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(token);
            if (principal == null)
                return BadRequest("Invalid token");

            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.GetUser(userId!);

            if(user == null)
            {
                return BadRequest("Invalid token");
            }

            var tokens = _jwtService.GenerateToken(user.Id);
            await _userService.SaveToken(user.Id, tokens.RefreshToken);

            return new RefreshTokenResponseDto
            {
                Id = user.Id,
                RefreshToken = tokens.RefreshToken,
                AccessToken = tokens.AccessToken
            };
        } 
    }
}
