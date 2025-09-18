using NoteMicroservice.Identity.Domain.Dto.BaseDtos;
using NoteMicroservice.Identity.Domain.Entities;

namespace NoteMicroservice.Identity.Domain.Dto;

public class GroupResponseDto : EntityDto
{
    public string Name  { get; set; }
    public int CountUser { get; set; }
    public List<UserResponseDto> Users { get; set; }
}

public static class GroupExtensions
{
    public static GroupResponseDto ToGroupResponseDto(this Group group)
    {
        var dto = new GroupResponseDto();
			
        dto.MapBaseProperties(group);
        dto.Name = group.Name;
        dto.Users = group.UserGroups?.Select(e => e.User.ToUserResponseDto()).ToList();
        dto.CountUser = group.UserGroups?.Count() ?? 0;
        return dto;
    }
    
    public static GroupResponseDto ToSearchGroupResponseDto(this Group group)
    {
        var dto = new GroupResponseDto();
			
        dto.MapBaseProperties(group);
        dto.Name = group.Name;
        dto.CountUser = group.UserGroups?.Count() ?? 0;
        return dto;
    }
}