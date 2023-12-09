using blog_backend.Entity.AccountEntities;
using blog_backend.Enums;

namespace blog_backend.Entity.CommunityEntities;

public class CommunityMembership
{
    public Guid UserId { get; set; }
    public Guid CommunityId { get; set; }

    public RoleEnum RoleEnum { get; set; } = RoleEnum.Subscriber;

    public User User { get; set; }
    public Community Community { get; set; }
}