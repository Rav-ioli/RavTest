using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using MyApplication.Data;
//using MyApplication.Domain;
using MyApplication.Dto;
using MyApplication.Controllers;
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
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/authentication", "Admin"));
    options.AddPolicy("BedrijfOnly", policy => policy.RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/authentication", "Bedrijf", "Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/authentication", "Admin", "Ervaringsdeskundige", "Bedrijf"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<ValidationService>();
builder.Services.AddScoped<UserService>();
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
                        await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Authentication, "Ervaringsdeskundige"));
                    }
                    // if (i == 2)
                    // {
                    //    await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Authentication, "admin"));
                    //     await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Authentication, "user"));
                    //     await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Authentication, "acteur"));
                        
                    //     await userService.CreateActorAsync(new UserDtoAdmin(){Username = user.UserName, Email = user.Email, TwoFactorEnabled = user.TwoFactorEnabled, Actor = "Sneed", Id = user.Id}, user);
                    // }
                    
                    // if (i == 5)
                    // {
                    //     await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Authentication, "admin"));
                    //     await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Authentication, "user"));
                    //     await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Authentication, "acteur"));
                        
                    //     await userService.CreateActorAsync(new UserDtoAdmin(){Username = user.UserName, Email = user.Email, TwoFactorEnabled = user.TwoFactorEnabled, Actor = "Ravish", Id = user.Id}, user);
                    // }
                    else
                    {
                        await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Authentication, "Ervaringsdeskundige"));
                    }
                }
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