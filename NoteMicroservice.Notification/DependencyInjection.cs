using Microsoft.Extensions.DependencyInjection;
using NoteMicroservice.Note.Domain.Abstract.Repository;
using NoteMicroservice.Note.Infrastructure.Repository;

namespace NoteMicroservice.Notification
{
	public static class DependencyInjection
	{
		public static IServiceCollection DependencyInjectionCore(this IServiceCollection services)
		{
			services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
			
			return services;
		}
	}
}
