using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NoteMicroservice.Note.API;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NoteDbContext = NoteMicroservice.Note.Domain.Context.NoteDbContext;

public class Program
{
    public static IServiceProvider Services { get; private set; }
    public static ILoggerFactory LoggerFactory { get; private set; }

    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        var configuration = builder.Configuration;

        builder.Services.AddDbContext<NoteDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        builder.Services.DependencyInjectionCore();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "NoteMicroservice.Note API", Version = "v1" });

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
        builder.AddServiceDefaults();

        builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.RequireHttpsMetadata = false;
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                };
            });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "NoteMicroservice.Note API V1"); });
        }

        app.MapControllerRoute(
            name: "default",
            pattern: "api/{controller}/{action=GetList}");

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.Run();
    }
}