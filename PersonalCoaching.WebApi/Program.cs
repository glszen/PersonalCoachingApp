using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PersonalCoaching.WebApi.GlobalExceptionHandling;
using PersonalCoaching.WebApi.Middleware;
using PersonalCoachingApp.Business.DataProtection;
using PersonalCoachingApp.Business.Operations.Feature;
using PersonalCoachingApp.Business.Operations.Package;
using PersonalCoachingApp.Business.Operations.Setting;
using PersonalCoachingApp.Business.Operations.User;
using PersonalCoachingApp.Data.Context;
using PersonalCoachingApp.Data.Repositories;
using PersonalCoachingApp.Data.UnitOfWork;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => 
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Name = "Jwt Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Put **_ONLY_** your Jwt Bearer Token on Textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            jwtSecurityScheme, Array.Empty<string>()
        }
    });
});

builder.Services.AddScoped<IDataProtection, DataProtection>();

var keysDirectiory = new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "App_Data", "Keys")); //We keep a key to unlock the passwords when the project is moved.

builder.Services.AddDataProtection()
    .SetApplicationName("PersonalCoachingApp")
    .PersistKeysToFileSystem(keysDirectiory);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) //For authentication.
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],

            ValidateLifetime = true, //Accepting expired tokens.

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
        };

    });

   
var connectionString = builder.Configuration.GetConnectionString("connect");

builder.Services.AddDbContext<CoachingAppDbContext>(options =>
    options.UseNpgsql(connectionString)
);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); //We are pulling it as a service, now it can be loaded using dependency injection.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUserService, UserManager>();

builder.Services.AddScoped<IFeatureService, FeatureManager>();

builder.Services.AddScoped<IPackageService, PackageManager>();

builder.Services.AddScoped<ISettingService, SettingManager>();

builder.Services.AddLogging();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>(); //Error handling middleware added.

app.UseMaintenanceMode();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
