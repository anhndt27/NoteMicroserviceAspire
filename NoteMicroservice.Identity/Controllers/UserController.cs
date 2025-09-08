using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NoteMicroservice.Identity.Domain.Abstract.Repository;
using NoteMicroservice.Identity.Domain.Abstract.Service;
using NoteMicroservice.Identity.Domain.Dto;
using NoteMicroservice.Identity.Domain.Dto.BaseDtos;
using NoteMicroservice.Identity.Domain.Extensions;
using NoteMicroservice.Identity.Domain.Resources;

namespace NoteMicroservice.Identity.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly IUserRepository _userRepository;
		private readonly ILogger<UserController> _logger; 
		private readonly IStringLocalizer<CommonTitles> _commonTitles;
		private readonly IStringLocalizer<CommonMessages> _commonMessages;
		
		public UserController(IUserService userService, IUserRepository userRepository, ILogger<UserController> logger, IStringLocalizer<CommonTitles> commonTitles, IStringLocalizer<CommonMessages> commonMessages)
		{
			_userService = userService;
			_userRepository = userRepository;
			_logger = logger;
			_commonTitles = commonTitles;
			_commonMessages = commonMessages;
		}
		
		[HttpPost("search")]
		public async Task<IActionResult> Search([FromBody] UserSearchRequestDto search)
		{
			try
			{
				var identityId = this.GetUserId();
				var res = await _userRepository.Search(identityId, search);
				return Ok(res);
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Exception when out group Dto: {@ReactGroupDto}", search);
				return this.InternalServerError(ResponseMessage.SomethingWrong(_commonTitles, _commonMessages));
			}
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(string id)
		{
			var res = await _userService.GetUserByIdAsync(id);
			return Ok(res);
		}
		
		[HttpGet("get-all")]
		public async Task<IActionResult> GetAll()
		{
			var res = await _userService.GetAllUsersAsync();
			return Ok(res);
		}

		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}
		
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
