using Microsoft.EntityFrameworkCore;
using IntentAPI.Models;
//using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Drawing.Printing;

var builder = WebApplication.CreateBuilder(args);

// Enable env variables
builder.Configuration.AddEnvironmentVariables();

//builder.Configuration.AddJsonFile("appsettings.json",
//        optional: true,
//        reloadOnChange: true);

//builder.Host.ConfigureAppConfiguration((configBuilder) =>
//{
//    configBuilder.Sources.Clear();
//    DotEnv.Load();
//    configBuilder.AddEnvironmentVariables();
//});

//var domain = $"https://{builder.Configuration.GetValue<string>("AUTH0_DOMAIN")}/";
var domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
System.Diagnostics.Debug.WriteLine("*****testagain**");
System.Diagnostics.Debug.WriteLine(domain);
// Add Auth0
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    //var audience = builder.Configuration.GetValue<string>("AUTH0_AUDIENCE");
    var audience = builder.Configuration["Auth0:Audience"];
    options.Authority = domain;
    options.Audience = audience;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = ClaimTypes.NameIdentifier
    };
});

builder.Services
  .AddAuthorization(options =>
  {
      options.AddPolicy("read:messages", policy => policy.Requirements.Add(
          new HasScopeRequirement("read:messages", domain)
        )
      );
  });

builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddDbContext<TodoContext>(opt =>
//    opt.UseInMemoryDatabase("TodoList")
//)

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure env

//var requiredVars =
//    new string[] {
//          "PORT",
//          "CLIENT_ORIGIN_URL",
//          "AUTH0_DOMAIN",
//          "AUTH0_AUDIENCE",
//    };

//foreach (var key in requiredVars)
//{
//    var value = app.Configuration.GetValue<string>(key);

//    if (value == "" || value == null)
//    {
//        throw new Exception($"Config variable missing: {key}.");
//    }
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
