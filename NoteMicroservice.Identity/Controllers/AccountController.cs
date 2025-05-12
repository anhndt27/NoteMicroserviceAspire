using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NoteMicroservice.Identity.Domain.Abstract.Service;
using NoteMicroservice.Identity.Domain.Dto;
using NoteMicroservice.Identity.Domain.Dto.BaseDtos;
using NoteMicroservice.Identity.Domain.Extensions;
using NoteMicroservice.Identity.Domain.Resources;

namespace NoteMicroservice.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly IAuthenticationsAsyncService _authenticationsAsyncService;
        private readonly ILogger<AccountController> _logger;
        private readonly IStringLocalizer<CommonTitles> _commonTitles;
        private readonly IStringLocalizer<CommonMessages> _commonMessages;

        public AccountController(IAuthenticationsAsyncService authenticationsAsyncService, ILogger<AccountController> logger, IStringLocalizer<CommonTitles> commonTitle, IStringLocalizer<CommonMessages> commonMessages)
        {
            _authenticationsAsyncService = authenticationsAsyncService;
            _logger = logger;
            _commonTitles = commonTitle;
            _commonMessages = commonMessages;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto model)
        {
            try
            {
                var result = await _authenticationsAsyncService.Register(model);
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Exception when register");
                return this.InternalServerError(ResponseMessage.SomethingWrong(_commonTitles, _commonMessages));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            try
            {
                var result = await _authenticationsAsyncService.Login(model);
                if (result.Token == "Fail") return BadRequest("Login fail");
				return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Exception when login");
                return this.InternalServerError(ResponseMessage.SomethingWrong(_commonTitles, _commonMessages));
            }
        }
    }
}
