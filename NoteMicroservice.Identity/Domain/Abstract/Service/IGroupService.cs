using NoteMicroservice.Identity.Domain.ViewModel;

namespace NoteMicroservice.Identity.Domain.Abstract.Service
{
    public interface IGroupService
    {
        Task<int> JoinGroup(JoinGroupViewModel request);
        Task<bool> OutGroup(ReactGroupViewModel request);
        Task<int> CreateGroup(GroupRequestViewModel request);
        Task<string> CreateCodeJoinGroup(int id);
    }
}
