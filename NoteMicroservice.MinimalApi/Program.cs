using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json")
    .AddEnvironmentVariables();

builder.Services.AddOcelot(builder.Configuration).AddPolly();;

builder.Services.AddSwaggerForOcelot(builder.Configuration);

builder.Services.AddSwaggerGen();


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

//var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

builder.AddServiceDefaults();

builder.Services.AddCors();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    app.UseSwaggerForOcelotUI(opt => {
        opt.PathToSwaggerGenerator = "/swagger/docs";
    });
}

app.UsePathBase("/gateway");

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseOcelot().Wait();

app.UseAuthorization();

app.UseCors(static builder => 
    builder.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());

app.MapControllers();

app.Run();
