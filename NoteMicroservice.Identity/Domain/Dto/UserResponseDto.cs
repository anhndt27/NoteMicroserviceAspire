using NoteMicroservice.Identity.Domain.Dto.BaseDtos;
using NoteMicroservice.Identity.Domain.Entities;

namespace NoteMicroservice.Identity.Domain.Dto
{
	public class UserResponseDto : EntityDto
	{
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<GroupResponseDto> Groups { get; set; }
    }

	public static class UserExtension
	{
		public static UserResponseDto ToUserResponseDto(this User user)
		{
			var dto = new UserResponseDto();
			
			dto.UserName = user.UserName;
			dto.Email = user.Email;
			dto.Groups = user.UserGroups?.Select(e => e.Group.ToGroupResponseDto()).ToList();
			dto.MapBaseProperties(user);
			return dto;
		}
	}
}
