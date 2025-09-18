using NoteMicroservice.Note.Domain.Abstract.Repository;
using NoteMicroservice.Note.Domain.Abstract.Service;
using NoteMicroservice.Note.Domain.Implement.Service;
using NoteMicroservice.Note.Domain.Repository;


namespace NoteMicroservice.Note.API
{
	public static class DependencyInjection
	{
		public static IServiceCollection DependencyInjectionCore(this IServiceCollection services)
		{
		

			return services;
		}
	}
}
