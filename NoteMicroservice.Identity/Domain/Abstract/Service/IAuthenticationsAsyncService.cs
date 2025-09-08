using NoteMicroservice.Identity.Domain.Dto;

namespace NoteMicroservice.Identity.Domain.Abstract.Service
{
    public interface IAuthenticationsAsyncService
    {
        Task<LoginResponseDto> Login(LoginRequestDto request);
        Task<string> Register(RegisterRequestDto request);
    }
}
