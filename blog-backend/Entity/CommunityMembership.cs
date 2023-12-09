using blog_backend.DAO.Utils;

namespace blog_backend.Entity;

public class CommunityMembership
{
    public Guid UserId { get; set; }
    public Guid CommunityId { get; set; }

    public RoleEnum RoleEnum { get; set; } = RoleEnum.Subscriber;

    public User User { get; set; }
    public Community Community { get; set; }
}