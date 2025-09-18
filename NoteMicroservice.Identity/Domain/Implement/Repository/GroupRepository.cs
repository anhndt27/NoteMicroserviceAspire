using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NoteMicroservice.Identity.Domain.Abstract.Repository;
using NoteMicroservice.Identity.Domain.Entities;
using NoteMicroservice.Identity.Domain.Dto;
using NoteMicroservice.Identity.Domain.Dto.BaseDtos;
using NoteMicroservice.Identity.Domain.Extensions;
using NoteMicroservice.Identity.Domain.Resources;
using NoteMicroservice.Identity.Infrastructure;

namespace NoteMicroservice.Identity.Domain.Implement.Repository;

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
			UserGroups = new List<UserGroups>()
		};
			
		group.UserGroups.Add(new UserGroups()
		{
			UserId = identityId,
		});

		_dbContext.Groups.Add(group);
		var changeCount = await _dbContext.SaveChangesAsync();
			
		if (changeCount > 0)
		{
			return ResponseMessage.AddedSuccess(_commonTitles, _commonMessages);
		}
		return ResponseMessage.NoRecordAdded(_commonTitles, _commonMessages);
	}

	public async Task<PaginatedListDto<GroupResponseDto>> SearchGroup(string identityId, GroupSearchRequestDto searchDto)
	{
		searchDto.TryCreateSingleQuery(_dbContext, out IQueryable<Group> query);
		var paginationDto = await query.CreatePaginationAsync(searchDto.PageIndex, searchDto.PageSize,
			e => e.ToSearchGroupResponseDto());

		return paginationDto;
	}

	public async Task<ResponseMessage> DeleteGroup(string identityId, string id)
	{
		var group = _dbContext.Groups.Include(e => e.UserGroups).FirstOrDefault(g => g.Id == id);
		if (group != null)
		{
			_dbContext.UserGroups.RemoveRange(group.UserGroups);
			_dbContext.Groups.Remove(group);
			var changeCount = _dbContext.SaveChanges();
			if (changeCount > 0)
			{
				return ResponseMessage.DeletedSuccess(_commonTitles, _commonMessages);
			}
			return  ResponseMessage.NoRecordDeleted(_commonTitles, _commonMessages);
		}
		return ResponseMessage.DataNotFound(_commonTitles, _commonMessages);
	}

	public async Task<List<GroupResponseDto>> GetAllGroups(string identityId)
	{
		var groups = await _dbContext.Groups
			.Where(e => e.UserGroups.Any(u => u.UserId == identityId))
			.Include(g => g.UserGroups)
			.ThenInclude(ug => ug.User)
			.Select(g => g.ToGroupResponseDto()).ToListAsync();

		if (!groups.Any())
		{
			return new List<GroupResponseDto>();
		}
			
		return groups;
	}

	public async Task<ResponseMessage> JoinGroup(string identityId, ReactGroupDto request)
	{
		var group = await _dbContext.Groups.Include(g => g.UserGroups).FirstOrDefaultAsync(x => x.Id == request.GroupId);

		if (group != null)
		{
			group.UserGroups.AddRange(request.UserIds.Select(e => new UserGroups()
			{
				UserId = e,
				Status = Status.Active
			}));
		}

		var changeCount = await _dbContext.SaveChangesAsync();
			
		if (changeCount > 0)
		{
			return ResponseMessage.UpdatedSuccess(_commonTitles, _commonMessages);
		}
		return ResponseMessage.NoRecordUpdated(_commonTitles, _commonMessages);
	}

	public async Task<ResponseMessage> OutGroup(string identityId, ReactGroupDto request)
	{
		var userGroups = await _dbContext.UserGroups
			.Where(x => x.GroupId == request.GroupId && request.UserIds.Contains(x.UserId))
			.ToListAsync();

		if (userGroups.Any())
		{
			_dbContext.UserGroups.RemoveRange(userGroups);
		}

		var changeCount = await _dbContext.SaveChangesAsync();
			
		if (changeCount > 0)
		{
			return ResponseMessage.UpdatedSuccess(_commonTitles, _commonMessages);
		}
		return ResponseMessage.NoRecordUpdated(_commonTitles, _commonMessages);
	}
}