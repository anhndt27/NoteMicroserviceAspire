using Serilog;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .WriteTo.File($"Logs/log.log")
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//var a = builder.Configuration["UrlConfiguration"];

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins, policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);
////Middleware
// app.UseMiddleware<CustomMiddleware>();

app.UseStaticFiles();

app.MapControllers();

app.Run();
