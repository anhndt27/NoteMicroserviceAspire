using NoteMicroservice.Note.Domain.Entity;

namespace NoteMicroservice.Note.Domain.Dtos;

public class AssignPermissionRequest
{
    public PrincipalType PrincipalType { get; set; }
    public string PrincipalId { get; set; }
    public Permissions PermissionType { get; set; }
    public AccessLevel AccessLevel { get; set; }
}