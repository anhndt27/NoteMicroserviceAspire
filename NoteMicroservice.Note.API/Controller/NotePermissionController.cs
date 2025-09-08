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

    [HttpPost]
    public async Task<IActionResult> AssignPermission(string noteContentId,
        [FromBody] AssignPermissionRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var requestingUserId = this.GetUserId();
        var requestingGroupId = this.GetGroupIds();
        if (string.IsNullOrEmpty(requestingUserId))
        {
            return Unauthorized();
        }

        try
        {
            await _permissionService.AssignNotePermissionAsync(
                requestingUserId,
                requestingGroupId,
                noteContentId,
                request.PrincipalType,
                request.PrincipalId,
                request.PermissionType,
                request.AccessLevel
            );

            return Ok(new { message = "Permission assigned successfully." });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500,
                "An error occurred while assigning permission.");
        }
    }

    [HttpDelete]
    public async Task<IActionResult> RevokePermission(string noteContentId,
        [FromBody] AssignPermissionRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var requestingUserId = this.GetUserId();
        var requestingGroupId = this.GetGroupIds();

        if (string.IsNullOrEmpty(requestingUserId))
        {
            return Unauthorized();
        }

        try
        {
            await _permissionService.RevokeNotePermissionAsync(
                requestingUserId,
                requestingGroupId,
                noteContentId,
                request.PrincipalType,
                request.PrincipalId,
                request.PermissionType
            );

            return Ok(new { message = "Permission revoked successfully." });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while revoking permission.");
        }
    }
}