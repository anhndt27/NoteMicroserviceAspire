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
			
        dto.Name = group.Name;
        dto.Users = group.UserGroups?.Select(e => e.User.ToUserResponseDto()).ToList();
        dto.MapBaseProperties(group);
        return dto;
    }
}