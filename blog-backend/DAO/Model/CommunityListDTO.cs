using blog_backend.DAO.Utils;

namespace blog_backend.DAO.Model;

public class CommunityListDTO
{
    public Guid UserId { get; set; }
    public Guid CommunityId { get; set; }
    public string Role { get; set; }
}