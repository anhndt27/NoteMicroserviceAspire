using NoteMicroservice.Identity.Domain.Abstract.Repository;
using NoteMicroservice.Identity.Domain.Abstract.Service;
using NoteMicroservice.Identity.Domain.Dto;

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
