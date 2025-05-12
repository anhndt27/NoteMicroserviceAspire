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
	public class GroupController : ControllerBase
	{
		private readonly IGroupService _groupService;
		private readonly IGroupRepository _groupRepository;
		private readonly ILogger<GroupController> _logger;
		private readonly IStringLocalizer<CommonTitles> _commonTitles;
		private readonly IStringLocalizer<CommonMessages> _commonMessages;
		public GroupController(IGroupService groupService, IGroupRepository groupRepository, ILogger<GroupController> logger, IStringLocalizer<CommonMessages> commonMessages, IStringLocalizer<CommonTitles> commonTitles)
		{
			_groupService = groupService;
			_groupRepository = groupRepository;
			_logger = logger;
			_commonMessages = commonMessages;
			_commonTitles = commonTitles;
		}

		[HttpPost("Create")]
		public async Task<IActionResult> CreateGroup(GroupRequestDto request)
		{
			try
			{
				var identityId = this.GetUserId();
				var res = await _groupRepository.CreateGroup(identityId, request);
				return Ok(res);
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Exception when create group Dto: {@GroupRequestDto}", request);
				return this.InternalServerError(ResponseMessage.SomethingWrong(_commonTitles, _commonMessages));
			}
		}
		
		[HttpPost("Search")]
		public async Task<IActionResult> CreateGroup(GroupSearchRequestDto request)
		{
			try
			{
				var identityId = this.GetUserId();
				var res = await _groupRepository.SearchGroup(identityId, request);
				return Ok(res);
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Exception when search group Dto: {@GroupSearchRequestDto}", request);
				return this.InternalServerError(ResponseMessage.SomethingWrong(_commonTitles, _commonMessages));
			}
		}

		[HttpPut("Join")]
		public async Task<IActionResult> JoinGroup(ReactGroupDto request)
		{
			try
			{
				var identityId = this.GetUserId();
				var res = await _groupRepository.JoinGroup(identityId, request);

				return Ok(res);
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Exception when join group Dto: {@ReactGroupDto}", request);
				return this.InternalServerError(ResponseMessage.SomethingWrong(_commonTitles, _commonMessages));
			}
		}

		[HttpPut("Out")]
		public async Task<IActionResult> OutGroup(ReactGroupDto request)
		{
			try
			{
				var identityId = this.GetUserId();
				var res = await _groupRepository.OutGroup(identityId, request);
				return Ok(res);
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Exception when out group Dto: {@ReactGroupDto}", request);
				return this.InternalServerError(ResponseMessage.SomethingWrong(_commonTitles, _commonMessages));
			}
		}

		[HttpGet("Get")]
		public async Task<IActionResult> GetGroupCode(int id)
		{
			try
			{
				var res = await _groupService.CreateCodeJoinGroup(id);
				return Ok(res);
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Exception when get group code Dto: {@id}", id);
				return this.InternalServerError(ResponseMessage.SomethingWrong(_commonTitles, _commonMessages));
			}
		}
	}
}
