using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NoteMicroservice.Note.Domain.Abstract.Service;
using NoteMicroservice.Note.Domain.Dto;
using OpenTelemetry.Trace;
using System.Diagnostics.Metrics;
using System;
using NoteMicroservice.Note.API.Extensions;
using NoteMicroservice.Note.Domain.Dtos;
using NoteMicroservice.Note.Domain.ViewModel;

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
        public async Task<IActionResult> Create(NoteRequestDto request)
        {
            try
            {
                var userId = this.GetUserId();
                
                var res = await _noteService.CreateNote(userId, request);
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
                var userId = this.GetUserId();
                var groupIds = this.GetGroupIds();

                var res = await _noteService.GetNote(id);
              
                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest("Fails");
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(string id, NoteReactDto request)
        {
            try
            {
                var userId = this.GetUserId();
                var groupIds = this.GetGroupIds();
                
                var res = await _noteService.UpdateNote(id, request);
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
                var groupIds = this.GetGroupIds();
                
                var res = await _noteService.DeleteNote(id);
                return Ok(res);
            }
            catch
            {
                return BadRequest("Fails");
            }
        }

	}
}