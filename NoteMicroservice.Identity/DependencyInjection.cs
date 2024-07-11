using NoteMicroservice.Identity.Domain.Abstract.Repository;
using NoteMicroservice.Identity.Domain.Abstract.Service;
using NoteMicroservice.Identity.Domain.Implement.Repository;
using NoteMicroservice.Identity.Domain.Implement.Service;

namespace NoteMicroservice.Identity
{
    public static class DependencyInjection
    {
        public static IServiceCollection DependencyInjectionCore(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthenticationsAsyncService, AuthenticationsAsyncService>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IGroupService, GroupService>();
            return services;
        }
    }
}
