using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NoteMicroservice.Identity.Domain.Abstract.Repository;
using NoteMicroservice.Identity.Domain.Entities;
using NoteMicroservice.Identity.Domain.Dto;
using NoteMicroservice.Identity.Domain.Dto.BaseDtos;
using NoteMicroservice.Identity.Domain.Resources;
using NoteMicroservice.Identity.Infrastructure;

namespace NoteMicroservice.Identity.Domain.Implement.Repository
{
	public class GroupRepository : IGroupRepository
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IStringLocalizer<CommonTitles> _commonTitles;
		private readonly IStringLocalizer<CommonMessages> _commonMessages;

		public GroupRepository(ApplicationDbContext dbContext, IStringLocalizer<CommonTitles> commonTitles, IStringLocalizer<CommonMessages> commonMessages)
		{
			_dbContext = dbContext;
			_commonTitles = commonTitles;
			_commonMessages = commonMessages;
		}

		public async Task<ResponseMessage> CreateGroup(string identityId, GroupRequestDto request)
		{
			var group = new Group()
			{
				Name = request.GroupName,
			};

			_dbContext.Groups.Add(group);
			var changeCount = await _dbContext.SaveChangesAsync();
			
			if (changeCount > 0)
			{
				return ResponseMessage.AddedSuccess(_commonTitles, _commonMessages);
			}
			return ResponseMessage.NoRecordAdded(_commonTitles, _commonMessages);
		}

		public Task<PaginatedListDto<GroupResponseDto>> SearchGroup(string identityId, GroupSearchRequestDto request)
		{
			throw new NotImplementedException();
		}

		public async Task<ResponseMessage> JoinGroup(string identityId, ReactGroupDto request)
		{
			var group = await _dbContext.Groups.Include(g => g.UserGroups).FirstOrDefaultAsync(x => x.Id == request.GroupId);

			if (group != null)
			{
				group.UserGroups.AddRange(request.UserIds.Select(e => new UserGroups()
				{
					UserId = e
				}));
			}

			var changeCount = await _dbContext.SaveChangesAsync();
			
			if (changeCount > 0)
			{
				return ResponseMessage.AddedSuccess(_commonTitles, _commonMessages);
			}
			return ResponseMessage.NoRecordAdded(_commonTitles, _commonMessages);
		}

		public async Task<ResponseMessage> OutGroup(string identityId, ReactGroupDto request)
		{
			var group = await _dbContext.Groups
			.Include(g => g.UserGroups)
			.FirstOrDefaultAsync(x => x.Id == request.GroupId);

			var changeCount = await _dbContext.SaveChangesAsync();
			
			if (changeCount > 0)
			{
				return ResponseMessage.AddedSuccess(_commonTitles, _commonMessages);
			}
			return ResponseMessage.NoRecordAdded(_commonTitles, _commonMessages);
		}

    }
}
