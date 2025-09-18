using System.Globalization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NoteMicroservice.Identity;
using NoteMicroservice.Identity.Domain.Entities;
using NoteMicroservice.Identity.Infrastructure;
using System.Text;
using Microsoft.AspNetCore.Localization;
using Microsoft.OpenApi.Models;
using NoteMicroservice.Identity.Domain.Abstract.Repository;
using NoteMicroservice.Identity.Domain.Abstract.Service;
using NoteMicroservice.Identity.Domain.Auths;
using NoteMicroservice.Identity.Domain.Configurations;
using NoteMicroservice.Identity.Domain.Constants;
using NoteMicroservice.Identity.Domain.Dto;
using NoteMicroservice.Identity.Domain.Implement.Repository;
using NoteMicroservice.Identity.Domain.Implement.Service;
using NoteMicroservice.ServiceDefaults;

public class Program
{
    public static IServiceProvider Services { get; private set; }
    public static ILoggerFactory LoggerFactory { get; private set; }

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Issuer"]
                };
            });
        
        // localization
        builder.Services.AddLocalization();
        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("vi"),
                new CultureInfo("en")
            };
            options.DefaultRequestCulture = new RequestCulture(culture: "vi", uiCulture: "vi");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "NoteMicroservice.Identity API", Version = "v1" });

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter JWT Token in the format 'Bearer {your token}'",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);

            var securityRequirement = new OpenApiSecurityRequirement
            {
                { securityScheme, new List<string>() }
            };
            c.AddSecurityRequirement(securityRequirement);
        });
        builder.Services.AddControllers();
        
        // DI
        builder.Services.AddScoped<IAuthenticationsAsyncService, AuthenticationsAsyncService>();
        builder.Services.AddScoped<IGroupRepository, GroupRepository >();
        builder.Services.AddScoped<IUserRepository, UserRepository >();
        
        builder.AddServiceDefaults();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "NoteMicroservice.Note API V1");
            });
        }

        app.MapControllerRoute(
            name: "default",
            pattern: "api/{controller}/{action=login}");

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.Run();
    }
}