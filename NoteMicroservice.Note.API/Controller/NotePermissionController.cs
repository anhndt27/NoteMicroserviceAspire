using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteMicroservice.Note.API.Extensions;
using NoteMicroservice.Note.Domain.Abstract.Repository;
using NoteMicroservice.Note.Domain.Abstract.Service;
using NoteMicroservice.Note.Domain.Dtos;
using NoteMicroservice.Note.Domain.Entity;

namespace NoteMicroservice.Note.API.Controller;

[Authorize]
[ApiController]
[Route("api/notes/{noteContentId}/permissions")]
public class NotePermissionController : ControllerBase
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly INotePermissionService _permissionService;

    public NotePermissionController(IPermissionRepository permissionRepository,
        INotePermissionService permissionService)
    {
        _permissionRepository = permissionRepository;
        _permissionService = permissionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPermissions(string noteContentId)
    {
        var requestingUserId = this.GetUserId();
 
        try
        {
            var permissions = await _permissionService.GetNotePermissionsAsync(
                requestingUserId, 
                noteContentId
            );
        
            return Ok(permissions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while retrieving permissions.");
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePermission([FromBody] PermissionRequestDto request)
    {
        var requestingUserId = this.GetUserId();
    
        try
        {
            await _permissionService.UpdateNotePermissionAsync(
                requestingUserId, request
            );

            return Ok(new { message = "Permission updated successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while updating permission.");
        }
    }
}