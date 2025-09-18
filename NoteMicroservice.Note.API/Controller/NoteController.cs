using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NoteMicroservice.Note.Domain.Abstract.Service;
using NoteMicroservice.Note.API.Extensions;
using NoteMicroservice.Note.Domain.Dtos;

namespace NoteMicroservice.Note.API.Controller
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;

		public NoteController(INoteService noteService)
		{
			_noteService = noteService;
		}

		[HttpPost("Search")]
		public async Task<IActionResult> GetListSearch(NoteSearchDto searchDto)
		{
			try
            {
                var userId = this.GetUserId();
                var groupIds = this.GetGroupIds();

                var res = await _noteService.Search(userId, groupIds, searchDto);
				return Ok(res);
			}
			catch
			{
				return BadRequest("Fails");
			}
		}

        [HttpPost("Create")]
        public async Task<IActionResult> Create(string groupId, NoteRequestDto request)
        {
            try
            {
                var userId = this.GetUserId();
                var email = this.GetEmail();
                
                var res = await _noteService.CreateNote(userId, email, groupId, request);
                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest("Fails");
            }
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetNote(string id)
        {
            try
            {
                var email = this.GetEmail();
                var groupIds = this.GetGroupIds();

                var res = await _noteService.GetNote(id, email, groupIds);
              
                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest("Fails");
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(string id, NoteRequestDto request)
        {
            try
            {
                var userId = this.GetUserId();
                var email = this.GetEmail();
                var groupIds = this.GetGroupIds();
                
                var res = await _noteService.UpdateNote(id, userId, email, groupIds, request);
                return Ok(res);
            }
            catch
            {
                return BadRequest("Fails");
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var userId = this.GetUserId();
                var email = this.GetEmail();
                var groupIds = this.GetGroupIds();
                
                var res = await _noteService.DeleteNote(id, userId, email, groupIds);
                return Ok(res);
            }
            catch
            {
                return BadRequest("Fails");
            }
        }
	}
}