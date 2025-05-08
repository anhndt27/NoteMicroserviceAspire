using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NoteMicroservice.Note.Domain.Abstract.Service;
using NoteMicroservice.Note.Domain.ViewModel;
using OpenTelemetry.Trace;
using System.Diagnostics.Metrics;
using System;

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

        // api
		[HttpGet("Search")]
		public async Task<IActionResult> GetListSearch(string userId, string groupId, string filter, string orderby)
		{
			try
			{
				var res = await _noteService.SearchListNotes(filter, orderby, userId, groupId);
				return Ok(res);
			}
			catch
			{
				return BadRequest("Fails");
			}
		}

		[HttpGet("GetList")]
        public async Task<IActionResult> GetListNotes(string userId, string groupId)
        {
            try
            {
                var res = await _noteService.GetListNotes(userId, groupId);
                return Ok(res);
            }
            catch
            {
                return BadRequest("Fails");
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(NoteRequestViewModel request)
        {
            try
            {
                var res = await _noteService.CreateNote(request);
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
                var res = await _noteService.GetNote(id);
              
                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest("Fails");
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(string id, NoteReactViewModel request)
        {
            try
            {
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
                var res = await _noteService.DeleteNote(id);
                return Ok(res);
            }
            catch
            {
                return BadRequest("Fails");
            }
        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(string id, [FromBody] UpdateCategoryRequest request)
        {
            try {
                var res = await _noteService.UpdateCategory(id, request);
                return Ok(res);
            }
            catch {
                return BadRequest("Fails");
            }
        }

		[HttpGet]
		[Route("Download")]
		public async Task<IActionResult> DownloadPdf()
		{
            var filePath = "C: \\Users\\Hi\\OneDrive - actvn.edu.vn\\Documents\\Workspace\\NoteMicroservice - master\\NoteMicroservice - master\\NoteMicroservice.Note.API\\file\\sample.pdf";
			
            if (!System.IO.File.Exists(filePath))
			{
				return NotFound();
			}

			var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

			return File(fileBytes, "application/pdf", "example.pdf");
		}

	}
}