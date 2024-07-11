using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteMicroservice.Identity.Domain.Abstract.Service;
using NoteMicroservice.Identity.Domain.Implement.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NoteMicroservice.Identity.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		// GET: api/<UserController>
		[HttpGet]
		public IEnumerable<string> Get()
		{
			return new string[] { "value1", "value2" };
		}

		// GET api/<UserController>/5
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(string id)
		{
			var res = await _userService.GetUserByIdAsync(id);
			return Ok(res);
		}

		// POST api/<UserController>
		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		// PUT api/<UserController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/<UserController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
