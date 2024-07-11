using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteMicroservice.Identity.Domain.Abstract.Service;
using NoteMicroservice.Identity.Domain.ViewModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NoteMicroservice.Identity.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class GroupController : ControllerBase
	{
		private readonly IGroupService _groupService;

		public GroupController(IGroupService groupService)
		{
			_groupService = groupService;
		}

		[HttpPost("Create")]
		public async Task<IActionResult> CreateGroup(GroupRequestViewModel request)
		{
			try
			{
				var res = await _groupService.CreateGroup(request);
				return Ok(res);
			}
			catch
			{
				return BadRequest("Fails");
			}
		}

		[HttpPut("Join")]
		public async Task<IActionResult> JoinGroup(JoinGroupViewModel request)
		{
			try
			{
				var res = await _groupService.JoinGroup(request);


				return Ok(res);
			}
			catch
			{
				return BadRequest(false);
			}
		}

		[HttpPut("Out")]
		public async Task<IActionResult> OutGroup(ReactGroupViewModel request)
		{
			try
			{
				var res = await _groupService.OutGroup(request);
				return Ok(res);
			}
			catch
			{
				return BadRequest(false);
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
			catch
			{
				return BadRequest("Fails");
			}
		}
	}
}
