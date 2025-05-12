using NoteMicroservice.Identity.Domain.Dto;

namespace NoteMicroservice.Identity.Domain.Abstract.Service
{
    public interface IGroupService
    {
        Task<string> CreateCodeJoinGroup(int id);
    }
}
