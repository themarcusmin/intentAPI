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

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins(builder.Configuration["Development:ClientURL"]).AllowAnyHeader().AllowAnyMethod();
        });
});

//var domain = $"https://{builder.Configuration.GetValue<string>("AUTH0_DOMAIN")}/";
var domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
System.Diagnostics.Debug.WriteLine("*****testagain**");
System.Diagnostics.Debug.WriteLine(domain);
// Add Auth0
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var audience = builder.Configuration["Auth0:Audience"];
    options.Authority = domain;
    options.Audience = audience;
});

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//.AddJwtBearer(options =>
//{
    //var audience = builder.Configuration.GetValue<string>("AUTH0_AUDIENCE");
    //
    //options.Authority = domain;
    //options.Authority = "http://dev-t4ir5j1c4g0g348z.au.auth0.com/";
    //options.Audience = "https://intent/api";
    //options.Audience = audience;
    //options.TokenValidationParameters = new TokenValidationParameters
    //{
    //    NameClaimType = ClaimTypes.NameIdentifier
    //};
//});

builder.Services
  .AddAuthorization(options =>
  {
      options.AddPolicy("create:event", policy => policy.Requirements.Add(
          new HasScopeRequirement("create:event", domain)
        )
      );
  });

builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
