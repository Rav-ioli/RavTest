using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MyApplication.Dto;
//using MyApplication.Domain;
using MyApplication.Services;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Microsoft.AspNetCore.Authorization;
using AccessibilityModels;
using MyApplication.Data;

namespace MyApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<Gebruiker> _userManager;
        private ApplicationDbContext _databaseContext;
        private readonly SignInManager<Gebruiker> _signInManager;
        private readonly AuthenticationService _authenticationService;
        private ValidationService _validationService;
        private readonly IAuthorizationService _authorizationService;


        public UserController(UserManager<Gebruiker> userManager, SignInManager<Gebruiker> signInManager, AuthenticationService authenticationService, IAuthorizationService authorizationService, ValidationService validationService, ApplicationDbContext databaseContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationService = authenticationService;
            _validationService = validationService;
            _authorizationService = authorizationService;
            _databaseContext = databaseContext;
        }

        // [HttpPost("RegisterUser")]
        // public async Task<ActionResult> RegisterNewUser([FromBody] RegisterDto registerDto)
        // {
        //     System.Console.WriteLine(_userManager);
        //     Console.WriteLine("Creating user");

        //     var user = new Gebruiker
        //     {
        //         UserName = registerDto.username,
        //         Email = registerDto.email,
        //         PasswordHash = registerDto.password,
        //     };

        //     var result = await _userManager.CreateAsync(user, user.PasswordHash);
        //     if (result.Succeeded)
        //     {
        //         await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Authentication, "Member"));
        //         Console.WriteLine("user created");

        //         var test = await _userManager.FindByNameAsync(user.UserName);
        //         Console.WriteLine($"USER: {test.UserName}");

        //         return Ok();
        //     }
        //     else
        //     {
        //         Console.WriteLine(result);
        //         return BadRequest();
        //     }
        // }

        [HttpPost("RegisterUser")]
        public async Task<ActionResult> RegisterNewUser([FromBody] RegisterDto registerDto)
        {
            System.Console.WriteLine(_userManager);
            Console.WriteLine("Creating user");

            var user = new Ervaringsdeskundige
            {
                UserName = registerDto.username,
                Email = registerDto.email,
                PasswordHash = registerDto.password,
            };

            var result = await _userManager.CreateAsync(user, user.PasswordHash);
            if (result.Succeeded)
            {
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Authentication, "Ervaringsdeskundige"));
                Console.WriteLine("user created");

                // var test = await _userManager.FindByNameAsync(user.UserName);
                // Console.WriteLine($"USER: {test.UserName}");

                return Ok();
            }
            else
            {
                Console.WriteLine(result);
                return BadRequest();
            }


            //  var ervaringsdeskundigeUser = new Ervaringsdeskundige
            // {
            //     UserName = "User" + i,
            //     Email = "user" + i + "@gmail.com",
            //     PasswordHash = "User123!",
            //     VoorkeurBenadering = "Telefonisch",
            //     MagCommercieelBenaderdWorden = true,
            //     GeboorteDatum = DateTime.Now.AddYears(-20),
            // };

            // if (!db.Gebruikers.Any(u => u.UserName == ervaringsdeskundigeUser.UserName))
            // {
            //     var result = await userManager.CreateAsync(ervaringsdeskundigeUser, ervaringsdeskundigeUser.PasswordHash);
            //     await userManager.AddClaimAsync(ervaringsdeskundigeUser, new Claim(ClaimTypes.Authentication, "Ervaringsdeskundige"));
            // }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {


            var expectedUser = await _authenticationService.GetUser(loginDto.email);

            if (expectedUser == null)
            {
                return NotFound("No user found");
            }

            // Check if the password is correct
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(expectedUser, loginDto.password);
            if (!isPasswordCorrect)
            {
                return Unauthorized("Invalid password");
            }

            await _signInManager.SignInAsync(expectedUser, true);

            return Ok(new { token = await _authenticationService.CreateJwtToken(expectedUser) });

        }

        [HttpPost("LoginGoogle")]
        public async Task<IActionResult> LoginGoogle([FromBody] GoogleDto googleDto)
        // {
        //     var expectedUser = await _authenticationService.GetUser(googleDto.email);

        //     if (expectedUser == null)
        //     {
        //         return NotFound("No user found");
        //     }

        //     await _signInManager.SignInAsync(expectedUser, true);

        //     return Ok(new { token = await _authenticationService.CreateJwtToken(expectedUser) });
        // }
        {
            System.Console.WriteLine(googleDto.audience);
            System.Console.WriteLine(googleDto.issuer);
            var expectedUser = await _authenticationService.GetUser(googleDto.email);
            if (googleDto.audience == "235973845509-5fddgbhrq2qs29am82tsr7unpch77gms.apps.googleusercontent.com" && googleDto.issuer == "https://accounts.google.com")
            {
                if (expectedUser == null)
                {
                    return NotFound("No user found");
                }

                await _signInManager.SignInAsync(expectedUser, true);

                return Ok(new { token = await _authenticationService.CreateJwtToken(expectedUser) });
            }
    return Unauthorized("Invalid token");
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

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("TestMethodPolicyAdminOnly")]
        public async Task<ActionResult> TestMethodPolicy()
        {
            System.Console.WriteLine("Method Called");
            return Ok();
        }




        [HttpPost("Validate")]
        public async Task<ActionResult> ValidateJwtRoles([FromBody] IList<string> roles)
        {
            return await _validationService.ValidateJwtClaims(roles, User) ? Ok(true) : Unauthorized(false);
        }
    }
}


