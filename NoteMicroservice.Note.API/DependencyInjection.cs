using NoteMicroservice.Note.Domain.Abstract.Repository;
using NoteMicroservice.Note.Domain.Abstract.Service;
using NoteMicroservice.Note.Domain.Implement.Service;
using NoteMicroservice.Note.Infrastructure.Repository;


namespace NoteMicroservice.Note.API
{
	public static class DependencyInjection
	{
		public static IServiceCollection DependencyInjectionCore(this IServiceCollection services)
		{
			services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
			services.AddScoped<INoteService, NoteService>();
			services.AddScoped<INoteRepository, NoteRepository>();
			return services;
		}
	}
}
