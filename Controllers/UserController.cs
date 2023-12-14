using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MyApplication.Dto;
//using MyApplication.Domain;
using MyApplication.Services;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace MyApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AuthenticationService _authenticationService;
        private ValidationService _validationService;
            private readonly IAuthorizationService _authorizationService;


        public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, AuthenticationService authenticationService,IAuthorizationService authorizationService, ValidationService validationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationService = authenticationService;
            _validationService = validationService;
            _authorizationService = authorizationService;
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

            return Ok(new { token = await _authenticationService.CreateJwtToken(expectedUser) });

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
        [HttpPost("TestMethod")]
        public async Task<ActionResult> TestMethod()
        {
            System.Console.WriteLine("Method Called");
            return Ok();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("TestMethodPolicy")]
        public async Task<ActionResult> TestMethodPolicy()
        {
            System.Console.WriteLine("Method Called");
            return Ok();
        }
        [HttpPost("TestMethodPolicy2")]
        public async Task<IActionResult> SomeAction()
        {
            // Check if the user is authorized based on a policy
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, null, "AdminOnly");
            if (!authorizationResult.Succeeded)
            {
                // User is not authorized
                return Forbid();
            }

            // User is authorized, proceed with the action logic
            System.Console.WriteLine("Authorized");
            return Ok("Authorized");
        }

        [HttpPost("Validate")]
        public async Task<ActionResult> ValidateJwtRoles([FromBody] IList<string> roles)
        {
            return await _validationService.ValidateJwtClaims(roles, User) ? Ok(true) : Unauthorized(false);
        }
    }
}


