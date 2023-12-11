using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MyApplication.Dto;
//using MyApplication.Domain;
using MyApplication.Services;
using Microsoft.EntityFrameworkCore;

namespace MyApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AuthenticationService _authenticationService;

        public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, AuthenticationService authenticationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationService = authenticationService;
        }

        [HttpPost("RegisterUser")]
        public async Task<ActionResult> RegisterNewUser([FromBody] RegisterDto registerDto)
        {
            System.Console.WriteLine(_userManager);
            Console.WriteLine("Creating user");

            var user = new IdentityUser
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

        [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        // if (!await _authenticationService.ValidateCredentials(loginDto.email, loginDto.password))
        //     return Unauthorized();

        var expectedUser = await _authenticationService.GetUser(loginDto.email);

        if (expectedUser == null)
        {
            return NotFound("No user found");
        }
        
        await _signInManager.SignInAsync(expectedUser, true);

        if(!expectedUser.TwoFactorEnabled) return Ok(new { token = await _authenticationService.CreateJwtToken(expectedUser)});

        // if (expectedUser.PhoneNumber == null) return BadRequest("User should have phone number for 2fa");
        
        // if (!expectedUser.PhoneNumberConfirmed) return BadRequest("Phone number should be confirmed");

        // await _authenticationService.SendTwoFactorAsync(expectedUser.PhoneNumber, expectedUser);
        return StatusCode(303, "mfa");
    }
    

        [HttpGet("GetUser")]
        public async Task<ActionResult<IdentityUser>> GetUser()
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
    [HttpPost("ReegisterUser")]
        public async Task<ActionResult> ReegisterNewUser()
        {
            System.Console.WriteLine("Method Called");
            return Ok();
        }
    }
}

       
    