using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoteMicroservice.Identity.Domain.Abstract.Service;
using NoteMicroservice.Identity.Domain.ViewModel;

namespace NoteMicroservice.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly IAuthenticationsAsyncService _authenticationsAsyncService;

        public AccountController(IAuthenticationsAsyncService authenticationsAsyncService)
        {
            _authenticationsAsyncService = authenticationsAsyncService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestViewModel model)
        {
            try
            {
                var result = await _authenticationsAsyncService.Register(model);
                return Ok(result);
            }
            catch
            {
                return BadRequest("User creation failed");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestViewModel model)
        {
            try
            {
                var result = await _authenticationsAsyncService.Login(model);
                if (result.Token == "Fail") return BadRequest("Login fail");
				return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("Login fail");
            }

        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authenticationsAsyncService.LogoutAsync();
            return Ok("User logged out successfully!");
        }
    }
}
