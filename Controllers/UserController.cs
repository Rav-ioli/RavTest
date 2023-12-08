using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MyApplication.Dto;
using MyApplication.Domain;
using Microsoft.EntityFrameworkCore;

namespace MyApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        public readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("RegisterUser")]
        public async Task<ActionResult> RegisterNewUser([FromBody] RegisterDto registerDto)
        {
            System.Console.WriteLine(_userManager);
            Console.WriteLine("Creating user");

            var user = new ApplicationUser
            {
                UserName = registerDto.username,
                Email = registerDto.email,
                PasswordHash = registerDto.password,
            };

            var result = await _userManager.CreateAsync(user, user.PasswordHash);
            if (result.Succeeded)
            {
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Authentication, "Member"));
                Console.WriteLine("user created");

                var test = await _userManager.FindByNameAsync(user.UserName);
                Console.WriteLine($"USER: {test.UserName}");

                return Ok();
            }
            else
            {
                Console.WriteLine(result);
                return BadRequest();
            }
        }

        [HttpGet("GetUser")]
        public async Task<ActionResult<ApplicationUser>> GetUser()
        {   
            var users = await _userManager.Users.ToListAsync();
            var firstUser = users.FirstOrDefault();

            if (firstUser != null)
            {
                return Ok(firstUser);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost("RegiawsterUser")]
        public async Task<ActionResult> test([FromBody] RegisterDto registerDto)
        {
            System.Console.WriteLine(_userManager);
            return Ok();
        }
    }
}

       
    