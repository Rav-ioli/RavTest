using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
//using MyApplication.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace MyApplication.Services;

public class AuthenticationService
{
    private  Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _userManager;
    private  IConfiguration _configuration;


    public AuthenticationService(Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    
    }

    public async Task<IdentityUser?> GetUser(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<string> CreateJwtToken(IdentityUser user)
    {
        var secret = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                "awef98awef978haweof8g7aw789efhh789awef8h9awh89efh89awe98f89uawef9j8aw89hefawef"));

        var signingCredentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim> { new(ClaimTypes.Name, user.Email) };

        var roles = await _userManager.GetClaimsAsync(user);
        claims.AddRange(roles);
        
        var tokenOptions = new JwtSecurityToken
        (
            issuer: _configuration.GetValue<string>("ValidIssuer"),
            audience: _configuration.GetValue<string>("ValidAudience"),
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: signingCredentials
        );

        var newToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return newToken;
    }

    public async Task<bool> ValidateCredentials(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user == null) return false;

        return await _userManager.CheckPasswordAsync(user, password);
    }

    // public async Task SendTwoFactorAsync(string phone, ApplicationUser user)
    // {
    //     phone = phone.Replace(" ", string.Empty);

    //     var pro = await _userManager.GetValidTwoFactorProvidersAsync(user);

    //     foreach (var pr in pro)
    //     {
    //         Console.WriteLine("PROVIDER: " + pr);
    //     }

    //     var verificationCode = await _userManager.GenerateTwoFactorTokenAsync(user, "Phone");

    //     await _smsService.SendAsync(new IdentityMessage
    //     {
    //         Destination = phone,
    //         Subject = "Laak verification message",
    //         Body = $"Your verification code is {verificationCode}"
    //     });

    //     Console.WriteLine("Send Message! " + verificationCode);
    // }
    
//     public async Task<bool> ValidateCodeAsync(string code)
//     {
//         return code != "";
//     }

//     public async Task<bool> CompareCodeAsync(string code, ApplicationUser user)
//     {
//         return await _userManager.VerifyTwoFactorTokenAsync(user, "Phone", code);
//     }
 }