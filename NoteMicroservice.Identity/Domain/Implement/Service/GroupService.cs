using NoteMicroservice.Identity.Domain.Abstract.Repository;
using NoteMicroservice.Identity.Domain.Abstract.Service;
using NoteMicroservice.Identity.Domain.ViewModel;

namespace NoteMicroservice.Identity.Domain.Implement.Service
{
	public class GroupService : IGroupService
	{
		private readonly IGroupRepository _groupRepository;
		private Dictionary<int, string> _codeDict = new Dictionary<int, string>();
		private Random _random = new Random();

		public GroupService(IGroupRepository groupRepository)
		{
			_groupRepository = groupRepository;
		}

		public async Task<string> CreateCodeJoinGroup(int id)
		{
			if (_codeDict.ContainsKey(id))
			{
				_codeDict[id] = await EncodeGroupCodeAsync(id);
			}
			else
			{
				_codeDict[id] = await EncodeGroupCodeAsync(id);
			}

			return _codeDict[id];
		}

		public async Task<int> CreateGroup(GroupRequestViewModel request)
		{
			var res = await _groupRepository.CreateGroup(request);
			return res;
		}

		public async Task<int> JoinGroup(JoinGroupViewModel request)
		{
			var groupId = await DecodeGroupCodeAsync(request.GroupCode);

			var res = await _groupRepository.JoinGroup(new ReactGroupViewModel()
			{
				GroupId = groupId,
				UserId = request.UserId,
			});
			return groupId;
		}

		public async Task<bool> OutGroup(ReactGroupViewModel request)
		{
			var res = await _groupRepository.OutGroup(request);
			return res;
		}

		public async Task<string> EncodeGroupCodeAsync(int groupId)
		{
			return await Task.Run(() =>
			{
				byte[] bytes = BitConverter.GetBytes(groupId);
				string groupCode = Convert.ToBase64String(bytes);
				return groupCode;
			});
		}

		public async Task<int> DecodeGroupCodeAsync(string groupCode)
		{
			return await Task.Run(() =>
			{
				byte[] bytes = Convert.FromBase64String(groupCode);
				int groupId = BitConverter.ToInt32(bytes, 0);
				return groupId;
			});
		}
	}
}
