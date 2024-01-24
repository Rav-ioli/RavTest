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


using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Text.Json.Serialization;
// using MyApplication.Domain;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

// builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContextConnection")));

builder.Services.AddDefaultIdentity<Gebruiker>(options => options.SignIn.RequireConfirmedAccount = false)
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
                    Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                    var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                    claimsIdentity?.AddClaim(new Claim("custom_claim", "some_value"));

                    return Task.CompletedTask;
                }
            };
        });

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
builder.Services.AddScoped<HulpmiddelService>();
builder.Services.AddScoped<BeperkingService>();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
// builder.Services.AddScoped<GoogleAudienceMiddleware>();
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
    var db = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    while (!db.Database.CanConnect())
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

    CreateUsersAsync(s, db).Wait();
    await CreateBeperkingenAsync(s, db);

    await CreateHulpmiddelenAsync(s, db);
    await CreateUserBeperkingenAsync(s, db);
    await CreateUserHulpmiddelenAsync(s, db);
    await CreateOnderzoekenAsync(s, db);
    await CreateGebruikerOnderzoeken(s, db);
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
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

app.Run();

async Task CreateUsersAsync(IServiceProvider s, ApplicationDbContext db)
{
    var userManager = s.GetRequiredService<UserManager<Gebruiker>>();

    for (int i = 1; i <= 10; i++)
    {
        if (i == 1)
        {
            // Create a Gebruiker
            var user = new Gebruiker
            {
                UserName = "User" + i,
                Email = "user" + i + "@gmail.com",
                PasswordHash = "User123!",
            };

            if (!db.Gebruikers.Any(u => u.UserName == user.UserName))
            {
                var result = await userManager.CreateAsync(user, user.PasswordHash);
                await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Authentication, "Admin"));
                await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Authentication, "Bedrijf"));
                await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Authentication, "Ervaringsdeskundige"));
            }
        }
        if (i == 2)
        {
            // Create a Bedrijf
            var bedrijfUser = new Bedrijf
            {
                UserName = "User" + i,
                Email = "user" + i + "@gmail.com",
                PasswordHash = "User123!",
                Bedrijfsnaam = "Microsoft",
                BedrijfsInformatie = "Microsoft Corporation is een Amerikaans bedrijf uit Redmond in Washington. Microsoft ontwikkelt, verspreidt, licentieert en ondersteunt een breed scala aan computergerelateerde producten.",
                // Set additional properties of Bedrijf here
            };

            if (!db.Gebruikers.Any(u => u.UserName == bedrijfUser.UserName))
            {
                await userManager.CreateAsync(bedrijfUser, bedrijfUser.PasswordHash);
                await userManager.AddClaimAsync(bedrijfUser, new Claim(ClaimTypes.Authentication, "Bedrijf"));
            }
        }
        if (i == 3)
        {
            // Create a Bedrijf
            var bedrijfUser = new Bedrijf
            {
                UserName = "User" + i,
                Email = "user" + i + "@gmail.com",
                PasswordHash = "User123!",
                Bedrijfsnaam = "Google",
                BedrijfsInformatie = "Google is een Amerikaans bedrijf dat zich specialiseert in internetdiensten en -producten, waaronder onlineadvertentie-technologieën, een zoekmachine, cloud-computing, software en hardware.",
                // Set additional properties of Bedrijf here
            };

            if (!db.Gebruikers.Any(u => u.UserName == bedrijfUser.UserName))
            {
                await userManager.CreateAsync(bedrijfUser, bedrijfUser.PasswordHash);
                await userManager.AddClaimAsync(bedrijfUser, new Claim(ClaimTypes.Authentication, "Bedrijf"));
            }

        }
        else
        {
            // Create an Ervaringsdeskundige
            var ervaringsdeskundigeUser = new Ervaringsdeskundige
            {
                UserName = "User" + i,
                Email = "user" + i + "@gmail.com",
                PasswordHash = "User123!",
                VoorkeurBenadering = "Telefonisch",
                MagCommercieelBenaderdWorden = true,
                GeboorteDatum = DateTime.Now.AddYears(-20),
            };

            if (!db.Gebruikers.Any(u => u.UserName == ervaringsdeskundigeUser.UserName))
            {
                var result = await userManager.CreateAsync(ervaringsdeskundigeUser, ervaringsdeskundigeUser.PasswordHash);
                await userManager.AddClaimAsync(ervaringsdeskundigeUser, new Claim(ClaimTypes.Authentication, "Ervaringsdeskundige"));
            }
        }
    }

}

async Task CreateOnderzoekenAsync(IServiceProvider s, ApplicationDbContext db)
{
    var onderzoekService = s.GetRequiredService<OnderzoekService>();
    var userService = s.GetRequiredService<UserService>();
    var beperkingService = s.GetRequiredService<BeperkingService>();
    var beperkingList = await beperkingService.GetBeperkingen();
    var random = new Random();
    // var randomBeperking = beperkingList[random.Next(beperkingList.Count)];

    await onderzoekService.ClearOnderzoekDB();
    string bedrijfId2 = await userService.GetUserIdByEmailAndDiscriminatorAsync("user2@gmail.com", "Bedrijf");
    string bedrijfId3 = await userService.GetUserIdByEmailAndDiscriminatorAsync("user3@gmail.com", "Bedrijf");
    var onderzoeken = new List<OnderzoekDto>
                {
                    new OnderzoekDto()
                    {
                        titel = "Onderzoek 1",
                        korteBeschrijving = "This is the first onderzoek",
                        datum = DateTime.Now.AddDays(-7),
                        beloning = "13250 euro",
                        soortOnderzoek = "Enquete",
                        uitvoerendbedrijf = bedrijfId2,
                        uitvoerendbedrijfnaam = userService.GetBedrijfsnaamById(bedrijfId2),
                        typebeperking = beperkingList[random.Next(beperkingList.Count)].BeperkingId
                    },
                    new OnderzoekDto()
                    {
                        titel = "Onderzoek 2",
                        korteBeschrijving = "This is the second onderzoek",
                        datum = DateTime.Now.AddDays(-7),
                        beloning = "1067 euro",
                        soortOnderzoek = "Enquete",
                        uitvoerendbedrijf = bedrijfId2,
                        uitvoerendbedrijfnaam = userService.GetBedrijfsnaamById(bedrijfId2),
                        typebeperking =  beperkingList[random.Next(beperkingList.Count)].BeperkingId
                    },
                    new OnderzoekDto()
                    {
                        titel = "Onderzoek 3",
                        korteBeschrijving = "This is the third onderzoek",
                        datum = DateTime.Now.AddDays(-7),
                        beloning = "1075 euro",
                        soortOnderzoek = "Fysiek",
                        uitvoerendbedrijf = bedrijfId2,
                        uitvoerendbedrijfnaam = userService.GetBedrijfsnaamById(bedrijfId2),
                        typebeperking =  beperkingList[random.Next(beperkingList.Count)].BeperkingId
                    },
                    new OnderzoekDto()
                    {
                        titel = "Onderzoek 4",
                        korteBeschrijving = "This is the fourth onderzoek",
                        datum = DateTime.Now.AddDays(-7),
                        beloning = "1 euro",
                        soortOnderzoek = "Fysiek",
                        uitvoerendbedrijf = bedrijfId3,
                        uitvoerendbedrijfnaam = userService.GetBedrijfsnaamById(bedrijfId3),
                        typebeperking = beperkingList[random.Next(beperkingList.Count)].BeperkingId
                    },
                    new OnderzoekDto()
                    {
                        titel = "Onderzoek 5",
                        korteBeschrijving = "In this fysiek onderzoek, you will take on the role of a cop who needs to devise a strategy to apprehend and neutralize a notorious robber. Use your detective skills, gather evidence, and plan your moves carefully to bring this criminal to justice.",
                        datum = DateTime.Now.AddDays(-7),
                        beloning = "1250 euro",
                        soortOnderzoek = "Fysiek",
                        uitvoerendbedrijf = bedrijfId3,
                        uitvoerendbedrijfnaam = userService.GetBedrijfsnaamById(bedrijfId3),
                        typebeperking = beperkingList[random.Next(beperkingList.Count)].BeperkingId
                    },
    new OnderzoekDto
    {
        titel = "Onderzoek 6",
        korteBeschrijving = "This is the sixth onderzoek",
        datum = DateTime.Now.AddDays(-14),
        beloning = "1800 euro",
        soortOnderzoek = random.Next(2) == 0 ? "Fysiek" : "Enquete",
        uitvoerendbedrijf = random.Next(2) == 0 ? bedrijfId2 : bedrijfId3,
        uitvoerendbedrijfnaam = userService.GetBedrijfsnaamById(random.Next(2) == 0 ? bedrijfId2 : bedrijfId3),
        typebeperking =  beperkingList[random.Next(beperkingList.Count)].BeperkingId
    },
    new OnderzoekDto
    {
        titel = "Onderzoek 7",
        korteBeschrijving = "This is the seventh onderzoek",
        datum = DateTime.Now.AddDays(-21),
        beloning = "1200 euro",
soortOnderzoek = random.Next(2) == 0 ? "Fysiek" : "Enquete",
        uitvoerendbedrijf = random.Next(2) == 0 ? bedrijfId2 : bedrijfId3,
        uitvoerendbedrijfnaam = userService.GetBedrijfsnaamById(random.Next(2) == 0 ? bedrijfId2 : bedrijfId3),
        typebeperking = beperkingList[random.Next(beperkingList.Count)].BeperkingId
    },
new OnderzoekDto()
{
    titel = "Onderzoek 8",
    korteBeschrijving = "This is the eighth onderzoek",
    datum = DateTime.Now.AddDays(-7),
    beloning = "1400 euro",
    soortOnderzoek = random.Next(2) == 0 ? "Fysiek" : "Enquete",
    uitvoerendbedrijf = random.Next(2) == 0 ? bedrijfId2 : bedrijfId3,
    uitvoerendbedrijfnaam = userService.GetBedrijfsnaamById(random.Next(2) == 0 ? bedrijfId2 : bedrijfId3),
    typebeperking =  beperkingList[random.Next(beperkingList.Count)].BeperkingId
},
new OnderzoekDto()
{
    titel = "Onderzoek 9",
    korteBeschrijving = "This is the ninth onderzoek",
    datum = DateTime.Now.AddDays(-7),
    beloning = "1500 euro",
    soortOnderzoek = random.Next(2) == 0 ? "Fysiek" : "Enquete",
    uitvoerendbedrijf = random.Next(2) == 0 ? bedrijfId2 : bedrijfId3,
    uitvoerendbedrijfnaam = userService.GetBedrijfsnaamById(random.Next(2) == 0 ? bedrijfId2 : bedrijfId3),
    typebeperking =  beperkingList[random.Next(beperkingList.Count)].BeperkingId
},new OnderzoekDto()
{
    titel = "Onderzoek 10",
    korteBeschrijving = "This is the tenth onderzoek",
    datum = DateTime.Now.AddDays(-7),
    beloning = "1600 euro",
    soortOnderzoek = random.Next(2) == 0 ? "Fysiek" : "Enquete",
    uitvoerendbedrijf = random.Next(2) == 0 ? bedrijfId2 : bedrijfId3,
    uitvoerendbedrijfnaam = userService.GetBedrijfsnaamById(random.Next(2) == 0 ? bedrijfId2 : bedrijfId3),
    typebeperking = beperkingList[random.Next(beperkingList.Count)].BeperkingId
},
new OnderzoekDto()
{
    titel = "Onderzoek 11",
    korteBeschrijving = "This is the eleventh onderzoek",
    datum = DateTime.Now.AddDays(-7),
    beloning = "1700 euro",
    soortOnderzoek = random.Next(2) == 0 ? "Fysiek" : "Enquete",
    uitvoerendbedrijf = random.Next(2) == 0 ? bedrijfId2 : bedrijfId3,
    uitvoerendbedrijfnaam = userService.GetBedrijfsnaamById(random.Next(2) == 0 ? bedrijfId2 : bedrijfId3),
    typebeperking = beperkingList[random.Next(beperkingList.Count)].BeperkingId
},
new OnderzoekDto()
{
    titel = "Onderzoek 12",
    korteBeschrijving = "This is the twelfth onderzoek",
    datum = DateTime.Now.AddDays(-7),
    beloning = "1800 euro",
    soortOnderzoek = "Enquete",
    uitvoerendbedrijf = random.Next(2) == 0 ? bedrijfId2 : bedrijfId3,
    uitvoerendbedrijfnaam = userService.GetBedrijfsnaamById(random.Next(2) == 0 ? bedrijfId2 : bedrijfId3),
    typebeperking = beperkingList[random.Next(beperkingList.Count)].BeperkingId
},
new OnderzoekDto()
{
    titel = "Onderzoek 13",
    korteBeschrijving = "This is the thirteenth onderzoek",
    datum = DateTime.Now.AddDays(-7),
    beloning = "1900 euro",
    soortOnderzoek = "Fysiek",
    uitvoerendbedrijf = random.Next(2) == 0 ? bedrijfId2 : bedrijfId3,
    uitvoerendbedrijfnaam = userService.GetBedrijfsnaamById(random.Next(2) == 0 ? bedrijfId2 : bedrijfId3),
    typebeperking =  beperkingList[random.Next(beperkingList.Count)].BeperkingId
}
                };
    foreach (var onderzoek in onderzoeken)
    {
        await onderzoekService.CreateOnderzoek(onderzoek);
    }
}
async Task CreateHulpmiddelenAsync(IServiceProvider s, ApplicationDbContext db)
{

    var hulpmiddelService = s.GetRequiredService<HulpmiddelService>();
    await hulpmiddelService.ClearHulpmiddelDB();


    var Hulpmiddelen = new List<HulpmiddelDto>
                {
                   new HulpmiddelDto { hulpmiddelnaam = "Blindenstok" },
                   new HulpmiddelDto { hulpmiddelnaam = "Schermlezer" },
                   new HulpmiddelDto { hulpmiddelnaam = "Vergrotingssoftware" },
                   new HulpmiddelDto { hulpmiddelnaam = "Contrastinstellingen" },
                   new HulpmiddelDto { hulpmiddelnaam = "Aangepaste lettertypes" },
                   new HulpmiddelDto { hulpmiddelnaam = "Toetsenbordnavigatie" },
                   new HulpmiddelDto { hulpmiddelnaam = "Spraakherkenning" },
                   new HulpmiddelDto { hulpmiddelnaam = "Tekst-naar-spraaktechnologie" },
                   new HulpmiddelDto { hulpmiddelnaam = "Aangepaste kleurenfilters" },
                   new HulpmiddelDto { hulpmiddelnaam = "Navigatie met spraakopdrachten" },
                   new HulpmiddelDto { hulpmiddelnaam = "Muisaanpassingen" },
                   new HulpmiddelDto { hulpmiddelnaam = "Tekstvergroter" },
                   new HulpmiddelDto { hulpmiddelnaam = "Dyslexie-vriendelijke lettertypes" }
                };

    foreach (var hulpmiddel in Hulpmiddelen)
    {
        await hulpmiddelService.CreateHulpmiddel(hulpmiddel);
    }

}
async Task CreateBeperkingenAsync(IServiceProvider s, ApplicationDbContext db)
{

    var beperkingService = s.GetRequiredService<BeperkingService>();
    await beperkingService.ClearBeperkingDB();

    var beperkingen = new List<BeperkingDto>
{
    new BeperkingDto { beperkingsnaam = "Doof" },
    new BeperkingDto { beperkingsnaam = "Blind" },
    new BeperkingDto { beperkingsnaam = "Rolstoelgebruiker" },
    new BeperkingDto { beperkingsnaam = "Spraakstoornis" },
    new BeperkingDto { beperkingsnaam = "Motorische beperking" },
    new BeperkingDto { beperkingsnaam = "Autisme spectrum stoornis" },
    new BeperkingDto { beperkingsnaam = "Verstandelijke beperking" },
    new BeperkingDto { beperkingsnaam = "Chronische ziekte" },
    new BeperkingDto { beperkingsnaam = "Psychische beperking" },
    new BeperkingDto { beperkingsnaam = "Visuele beperking" }
};

    foreach (var beperking in beperkingen)
    {
        await beperkingService.CreateBeperking(beperking);
    }
}
async Task CreateUserBeperkingenAsync(IServiceProvider s, ApplicationDbContext db)
{

    var userService = s.GetRequiredService<UserService>();
    var beperkingService = s.GetRequiredService<BeperkingService>();

    // Define the emails of the users you want to process
    var emails = new List<string> { "user4@gmail.com", "user5@gmail.com","user6@gmail.com","user7@gmail.com","user8@gmail.com","user9@gmail.com","user10@gmail.com" };

    // Get all users and filter them by their emails
    var gebruikers = (await userService.GetAllUsersAsync())
        .Where(u => emails.Contains(u.Email));

    var beperkingen = await beperkingService.GetBeperkingen();
    var random = new Random();

    foreach (var gebruiker in gebruikers)
    {
        var beperkingenCount = random.Next(2, 5);
        var beperkingenList = new List<Beperking>();

        for (int i = 0; i < beperkingenCount; i++)
        {
            var beperking = beperkingen[random.Next(0, beperkingen.Count)];
            if (!beperkingenList.Contains(beperking))
            {
                beperkingenList.Add(beperking);
            }
        }

        foreach (var beperking in beperkingenList)
        {
            await beperkingService.CreateUserBeperkingAsync(gebruiker.Id, beperking.BeperkingId);
        }
    }
}
async Task CreateUserHulpmiddelenAsync(IServiceProvider s, ApplicationDbContext db)
{
    var userService = s.GetRequiredService<UserService>();
    var hulpmiddelService = s.GetRequiredService<HulpmiddelService>();

    // Define the emails of the users you want to process
    var emails = new List<string> { "user4@gmail.com", "user5@gmail.com","user6@gmail.com","user7@gmail.com","user8@gmail.com","user9@gmail.com","user10@gmail.com"  };

    // Get all users and filter them by their emails
    var gebruikers = (await userService.GetAllUsersAsync())
        .Where(u => emails.Contains(u.Email));

    var hulpmiddelen = await hulpmiddelService.GetHulpmiddelen();
    var random = new Random();

    foreach (var gebruiker in gebruikers)
    {
        var hulpmiddelenCount = random.Next(2, 5);
        var hulpmiddelenList = new List<Hulpmiddel>();

        for (int i = 0; i < hulpmiddelenCount; i++)
        {
            var hulpmiddel = hulpmiddelen[random.Next(0, hulpmiddelen.Count)];
            if (!hulpmiddelenList.Contains(hulpmiddel))
            {
                hulpmiddelenList.Add(hulpmiddel);
            }
        }

        foreach (var hulpmiddel in hulpmiddelenList)
        {
            await hulpmiddelService.CreateUserHulpmiddelAsync(gebruiker.Id, hulpmiddel.HulpmiddelId);
        }
    }
}
async Task CreateGebruikerOnderzoeken(IServiceProvider s, ApplicationDbContext db){
    var userService = s.GetRequiredService<UserService>();
    var onderzoekService = s.GetRequiredService<OnderzoekService>();
    var emails = new List<string> {"user6@gmail.com","user7@gmail.com","user8@gmail.com","user9@gmail.com","user10@gmail.com"  };
    var gebruikers = (await userService.GetAllUsersAsync())
        .Where(u => emails.Contains(u.Email));

        var onderzoeken = await onderzoekService.GetOnderzoeken();
        var random = new Random();
        foreach(var gebruiker in gebruikers){
            var onderzoekenCount = random.Next(2,5);
            var onderzoekenList = new List<Onderzoek>();
            for(int i = 0; i < onderzoekenCount; i++){
                var onderzoek = onderzoeken[random.Next(0, onderzoeken.Count)];
                if(!onderzoekenList.Contains(onderzoek)){
                    onderzoekenList.Add(onderzoek);
                }
            }
            foreach(var onderzoek in onderzoekenList){
                await onderzoekService.CreateUserOnderzoekAsync(gebruiker.Id, onderzoek.OnderzoekId);
            }
        }
}