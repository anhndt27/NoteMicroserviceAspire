using NoteMicroservice.Note.Domain.Entity;

namespace NoteMicroservice.Note.Domain.Abstract.Repository;

public interface IPermissionRepository
{
    Task<bool> HasAccessAsync(string noteId, string email, List<string> groupIds);
}