using Microsoft.EntityFrameworkCore;
using NoteMicroservice.Note.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

builder.Services.AddDbContext<NoteDbContext>(optionsAction =>
{
    optionsAction.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("NoteMicroservice.Note.EFCore"));
});

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();
