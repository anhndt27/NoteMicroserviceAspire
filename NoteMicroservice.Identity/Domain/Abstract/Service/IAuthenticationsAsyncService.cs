using NoteMicroservice.Identity.Domain.ViewModel;

namespace NoteMicroservice.Identity.Domain.Abstract.Service
{
    public interface IAuthenticationsAsyncService
    {
        Task<LoginResponseViewModel> Login(LoginRequestViewModel request);
        Task<string> Register(RegisterRequestViewModel request);
        Task LogoutAsync();
    }
}
