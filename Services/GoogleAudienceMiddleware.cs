using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Google.Apis.Auth;
namespace MyApplication.Services;
public class GoogleAudienceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _expectedAudience;

    public GoogleAudienceMiddleware(RequestDelegate next, string expectedAudience)
    {
        _next = next;
        _expectedAudience = expectedAudience;
    }

    public async Task Invoke(HttpContext context)
    {
        var idToken = context.Request.Headers["Authorization"]; // Veronderstel dat het idToken in de Authorization-header zit

        if (string.IsNullOrEmpty(idToken))
        {
            context.Response.StatusCode = 401; // Ongeautoriseerd
            return;
        }

        try
        {
            var payload = GoogleJsonWebSignature.ValidateAsync(idToken).Result;
            //signature valideren(public key van google)
            

            // Controleer of de "audience" overeenkomt met de verwachte waarde
            if (payload.Audience != _expectedAudience)
            {
                context.Response.StatusCode = 403; // Verboden
                return;
            }
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500; // Interne serverfout
            return;
        }

        await _next(context);
    }
}
