using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
//using MyApplication.Domain;
using MyApplication.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using MyApplication.Services;

namespace MyApplication.Services;

public class UserService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly AuthenticationService _authenticationService;
    private readonly IConfiguration _configuration;

    public UserService(UserManager<IdentityUser> userManager, IConfiguration configuration, AuthenticationService authenticationService)
    {
        _userManager = userManager;
        _configuration = configuration;
        _authenticationService = authenticationService;
    }

    public string[] GetClaims()
    {
        string[] claims = { "user", "admin", "acteur" };

        return claims;
    }

    public async Task<List<UserDtoAdmin>> GetAllUsersAsync()
    {
        var users = await _userManager.Users.ToArrayAsync();

        var result = new List<UserDtoAdmin>();

        foreach (var applicationUser in users)
        {
            var claims = await _userManager.GetClaimsAsync(applicationUser);

            result.Add(new UserDtoAdmin
            {
                Username = applicationUser.UserName,
                Email = applicationUser.Email,
                TwoFactorEnabled = applicationUser.TwoFactorEnabled,
                Phone = applicationUser.PhoneNumber,
                claims = claims.Select(c => c.Value).ToArray(),
                Id = applicationUser.Id
            });
        }

        return result;
    }

    public async Task<UserDtoAdmin> GetUserDtoAsync(IdentityUser applicationUser)
    {
        var claims = await _userManager.GetClaimsAsync(applicationUser);

        var result = new UserDtoAdmin
        {
            Username = applicationUser.UserName,
            Email = applicationUser.Email,
            TwoFactorEnabled = applicationUser.TwoFactorEnabled,
            Phone = applicationUser.PhoneNumber,
            claims = claims.Select(c => c.Value).ToArray(),
            Id = applicationUser.Id
        };

        return result;
    }

    public async Task<IdentityUser?> GetUserByIIdentityAsync(IIdentity? userClaim)
    {
        if (userClaim?.Name == null)
        {
            return null;
        }

        return await GetUserByUsernameAsync(userClaim.Name);
    }

    public async Task<IdentityUser?> GetUserByIdAsync(Guid id)
    {
        var expectedUser = await _userManager.FindByIdAsync(id.ToString());

        return expectedUser;
    }


    public async Task<IdentityUser?> GetUserByUsernameAsync(string username)
    {
        return await _userManager.FindByNameAsync(username);
    }

    public async Task DeleteUserAsync(IdentityUser user)
    {
        await _userManager.DeleteAsync(user);
    }

    public async Task<bool> DisableAccountAsync(IdentityUser user)
    {
        user.LockoutEnabled = true;

        await _userManager.UpdateAsync(user);

        return true;
    }

    //dont know if old claims have to be removed first
    public async Task AddClaimsAsync(IdentityUser user, string[] claims)
    {
        foreach (var claim in claims)
        {
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Authentication, claim));
        }
    }
    
    public async Task<IList<Claim>> GetUserClaimsAsync(IdentityUser expectedUser)
    {
        var result = await _userManager.GetClaimsAsync(expectedUser);

        return result;
    }
    
    public async Task UpdateClaimsAsync(IdentityUser expectedUser, string[] userDtoClaims, IList<Claim> oldClaims)
    {
        Console.WriteLine("Remove claims");
        foreach (var oldClaim in oldClaims)
        {
            Console.WriteLine($"CLAIM: {oldClaim}");
        }
        
        var toRemove = oldClaims.Where(claim => !userDtoClaims.Contains(claim.Value)).ToArray();

        await _userManager.RemoveClaimsAsync(expectedUser, toRemove);

        Console.WriteLine("add claims");
        foreach (var oldClaim in userDtoClaims)
        {
            Console.WriteLine($"CLAIM: {oldClaim}");
        }
        
        await AddClaimsAsync(expectedUser, userDtoClaims);
    }

    public async Task UpdateUserAsync(IdentityUser expectedUser, UserDtoAdmin userDto)
    {
        expectedUser.Email = userDto.Email;
        expectedUser.UserName = userDto.Username;
        expectedUser.PhoneNumber = userDto.Phone;
        expectedUser.TwoFactorEnabled = userDto.TwoFactorEnabled;

        await _userManager.UpdateAsync(expectedUser);
    }

    public async Task<IdentityUser> CreateUserAsync(NewUserDtoAdmin userDto)
    {
        var newUser = new IdentityUser
        {
            Email = userDto.Email,
            UserName = userDto.Username,
            PhoneNumber = userDto.Phone,
            TwoFactorEnabled = userDto.TwoFactorEnabled,
            PasswordHash = userDto.UnhashedPassword
        };

        await _userManager.CreateAsync(newUser, newUser.PasswordHash);

        return newUser;
    }
}