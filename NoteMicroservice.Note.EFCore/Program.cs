using Microsoft.EntityFrameworkCore;
using NoteDbContext = NoteMicroservice.Note.Domain.Context.NoteDbContext;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

builder.Services.AddDbContext<NoteDbContext>(optionsAction =>
{
    optionsAction.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("NoteMicroservice.Note.EFCore"));
});

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();
