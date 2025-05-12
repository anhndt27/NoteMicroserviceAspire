using NoteMicroservice.Identity.Domain.Dto.BaseDtos;
using NoteMicroservice.Identity.Domain.Entities;
using NoteMicroservice.Identity.Infrastructure;

namespace NoteMicroservice.Identity.Domain.Dto;

public class UserRequestDto
{
    
}

public class UserSearchRequestDto : SearchRequestDto<User>
{
    public override List<string> OrderByValues { get; }
    public override bool TryCreateSingleQuery(ApplicationDbContext context, out IQueryable<User> query)
    {
        throw new NotImplementedException();
    }
}