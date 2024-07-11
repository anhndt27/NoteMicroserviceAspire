using NoteMicroservice.Identity.Domain.ViewModel;

namespace NoteMicroservice.Identity.Domain.Abstract.Repository
{
	public interface IGroupRepository
	{
		Task<bool> JoinGroup(ReactGroupViewModel request);
		Task<bool> OutGroup(ReactGroupViewModel request);
		Task<int> CreateGroup(GroupRequestViewModel request);
	}
}
