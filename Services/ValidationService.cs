using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MyApplication.Services;

public class ValidationService
{

    private readonly IAuthorizationService _authorizationService;
    private readonly IAuthorizationPolicyProvider _authorizationPolicyProvider;

    public ValidationService(IAuthorizationPolicyProvider authorizationPolicyProvider, IAuthorizationService authorizationService)
    {
        _authorizationPolicyProvider = authorizationPolicyProvider;
        _authorizationService = authorizationService;
    }
    
    public async Task<bool> ValidateJwtClaims(IList<string> claims, ClaimsPrincipal userClaim)
    {
        foreach (var policyName in claims)
        {
            var policy = await _authorizationPolicyProvider.GetPolicyAsync(policyName);
            System.Console.WriteLine($"Policy: {policy} and PolicyName: {policyName}");
            if (policy == null)
            {
                continue;
            }
            
            var result = await _authorizationService.AuthorizeAsync(userClaim, policy);

            if (result.Succeeded)
            {
                return true;
            }
        }

        return false;
    }

//     public async Task<bool> ValidateJwtClaims(IList<string> requiredRoles, ClaimsPrincipal userClaim)
// {
//     // Extract the claim values from the ClaimsPrincipal object
//     var claimValues = userClaim.Claims
//         .Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
//         .Select(c => c.Value)
//         .ToList();

//     foreach (var requiredRole in requiredRoles)
//     {
//         // Check if any of the claim values match the required roles
//         if (claimValues.Contains(requiredRole))
//         {
//             return true;
//         }
//     }

//     return false;
// }

}