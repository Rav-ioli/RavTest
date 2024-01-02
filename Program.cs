using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using MyApplication.Data;
//using MyApplication.Domain;
using MyApplication.Dto;
using MyApplication.Controllers;
using AccessibilityModels;
using Microsoft.AspNetCore.Authentication.JwtBearer; // Add this using directive
using Microsoft.AspNetCore.Authentication.BearerToken;
using MyApplication.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();


 builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowReactApp",
            builder =>
            {
                builder.WithOrigins("http://localhost:3000") // Replace with your React app's URL
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });

//builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();



// builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
// .AddEntityFrameworkStores<ApplicationDbContext>()
// .AddDefaultTokenProviders();

// builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
//     options.SignIn.RequireConfirmedAccount = true)
//    .AddRoles<IdentityRole>()
//     .AddEntityFrameworkStores<ApplicationDbContext>()
//     .AddDefaultTokenProviders();



// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle



builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Update the scheme to JwtBearerDefaults.AuthenticationScheme
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Update the scheme to JwtBearerDefaults.AuthenticationScheme
}).AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = builder.Configuration.GetValue<string>("ValidIssuer"),
                ValidAudience = builder.Configuration.GetValue<string>("ValidAudience"),

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            "awef98awef978haweof8g7aw789efhh789awef8h9awh89efh89awe98f89uawef9j8aw89hefawef"))
            };
            opt.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    // Handle failed authentication
                    Console.WriteLine("OnAuthenticationFailed: " +
                                      context.Exception.Message);
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    // Transform claims or additional logic after successful validation
                    // For example, adding custom claims to the identity
                    Console.WriteLine("OnTokenValidated: " +
                                      context.SecurityToken);
                    var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                    claimsIdentity?.AddClaim(new Claim("custom_claim", "some_value"));
 
                    return Task.CompletedTask;
                }
            };
        });


// builder.Services.AddAuthorization(options =>
//         {
//             options.AddPolicy("AdminOnly", x => x.RequireClaim(ClaimTypes.Authentication, "Admin"));
//             options.AddPolicy("BedrijfOnly", x => x.RequireClaim(ClaimTypes.Authentication, "Bedrijf","Admin"));
//             options.AddPolicy("UserOnly", x => x.RequireClaim(ClaimTypes.Authentication, "Admin", "Ervaringsdeskundige", "Bedrijf"));
//         });

// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
//     options.AddPolicy("BedrijfOnly", policy => policy.RequireRole("Bedrijf", "Admin"));
//     options.AddPolicy("UserOnly", policy => policy.RequireRole("Admin", "Ervaringsdeskundige", "Bedrijf"));
// });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim(ClaimTypes.Authentication, "Admin"));
    options.AddPolicy("BedrijfOnly", policy => policy.RequireClaim(ClaimTypes.Authentication, "Bedrijf", "Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireClaim(ClaimTypes.Authentication, "Admin", "Ervaringsdeskundige", "Bedrijf"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<ValidationService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<OnderzoekService>();
var app = builder.Build();
 app.UseCors("AllowReactApp");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




 using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var db = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database;

            while (!db.CanConnect())
            {
                Thread.Sleep(1000);
            }
            try
            {
                serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
            }
            catch (Exception ex)
            {
            }

            var s = app.Services.CreateScope().ServiceProvider;

            var userManager = s.GetRequiredService<UserManager<IdentityUser>>();
            var userService = s.GetRequiredService<UserService>();

            for (int i = 1; i <= 5; i++)
            {
                var user = new IdentityUser
                {
                    UserName = "User" + i,
                    Email = "user" + i + "@gmail.com",
                    PasswordHash = "User123!",
                };

                var result = await userManager.CreateAsync(user, user.PasswordHash);
                if (result.Succeeded)
                {
                    
                    if (i == 1)

                    {                      
                        await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Authentication, "Admin"));
                      await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Authentication, "Bedrijf"));
                        await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Authentication, "Ervaringsdeskundige"));
                    }
                    if (i == 2)
                    {
                        await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Authentication, "Bedrijf"));
                        await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Authentication, "Ervaringsdeskundige"));
                    }
                    if(i == 3)
                    {
                        await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Authentication, "Bedrijf"));
                    }
                    else
                    {
                        await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Authentication, "Ervaringsdeskundige"));
                    }
                }
            }
        var onderzoekService = s.GetRequiredService<OnderzoekService>();
        await onderzoekService.ClearOnderzoekDB();

                var onderzoeken = new List<OnderzoekDto>
                {
                    new OnderzoekDto()
                    {
                        titel = "Onderzoek 1",
                        korteBeschrijving = "This is the first onderzoek",
                        datum = DateTime.Now.AddDays(-7),
                        beloning = "13250 euro",
                        soortOnderzoek = "Enquete"
                    },
                    new OnderzoekDto()
                    {
                        titel = "Onderzoek 2",
                        korteBeschrijving = "This is the second onderzoek",
                        datum = DateTime.Now.AddDays(-7),
                        beloning = "1067 euro",
                        soortOnderzoek = "Enquete"
                    },
                    new OnderzoekDto()
                    {
                        titel = "Onderzoek 3",
                        korteBeschrijving = "This is the third onderzoek",
                        datum = DateTime.Now.AddDays(-7),
                        beloning = "1075 euro",
                        soortOnderzoek = "Fysiek"
                    },
                    new OnderzoekDto()
                    {
                        titel = "Onderzoek 4",
                        korteBeschrijving = "This is the fourth onderzoek",
                        datum = DateTime.Now.AddDays(-7),
                        beloning = "1 euro",
                        soortOnderzoek = "Fysiek"
                    },
                    new OnderzoekDto()
                    {
                        titel = "Onderzoek 5",
                        korteBeschrijving = "In this fysiek onderzoek, you will take on the role of a cop who needs to devise a strategy to apprehend and neutralize a notorious robber. Use your detective skills, gather evidence, and plan your moves carefully to bring this criminal to justice.",
                        datum = DateTime.Now.AddDays(-7),
                        beloning = "1250 euro",
                        soortOnderzoek = "Fysiek"
                    }
                };
foreach (var onderzoek in onderzoeken){
    await onderzoekService.CreateOnderzoek(onderzoek);
}
            


            app.UseSwagger();
            app.UseSwaggerUI();
        }







app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
//builder.Services.AddControllers();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "Bedrijf", "Ervaringsdeskundige" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

// using (var scope = app.Services.CreateScope())
// {
//     var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
//     var email = "admin@localhost";
//     var password = "Admin123!";
//     if (!await userManager.Users.AnyAsync())
//     {
//         var adminUser = new IdentityUser()
//         {
//             UserName = email,
//             Email = email
//         };
//         var result = await userManager.CreateAsync(adminUser, password);
//         System.Console.WriteLine(result.Succeeded);
//         var result2 = await userManager.AddToRoleAsync(adminUser, "Admin");
//         System.Console.WriteLine(result2.Succeeded);
//     }
// }

app.Run();