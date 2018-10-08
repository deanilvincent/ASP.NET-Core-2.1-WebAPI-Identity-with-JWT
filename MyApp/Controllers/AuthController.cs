using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Dtos;
using MyApp.Models;
using MyApp.Services;

namespace MyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IJwtTokenGenerator jwtTokenGenerator;

        public AuthController(IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var user = await userManager.FindByNameAsync(userForLoginDto.UserName);

            if (user == null) return BadRequest(new
            {
                result = "No user has found"
            });

            var result = await signInManager.CheckPasswordSignInAsync(user, userForLoginDto.Password, false);

            if (!result.Succeeded) return Unauthorized();
            {
                var appUser = await userManager.Users.FirstOrDefaultAsync(u =>
                    u.NormalizedUserName == userForLoginDto.UserName.ToUpper());

                var userToReturn = mapper.Map<User>(appUser);

                return Ok(new
                {
                    token = await jwtTokenGenerator.GenerateJwtTokenString(userToReturn)
                });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            var userToCreate = mapper.Map<User>(userForRegisterDto);

            var result = await userManager.CreateAsync(userToCreate, userForRegisterDto.Password);
            await userManager.AddToRoleAsync(userToCreate, "NormalUser");

            if (!result.Succeeded) return BadRequest();
            return Ok("Successfully registered");
        }
    }
}