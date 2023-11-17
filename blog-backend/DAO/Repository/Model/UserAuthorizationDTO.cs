using blog_backend.DAO.Repository.Utils;

namespace blog_backend.Dao.Repository.Model;

public class UserAuthorizationDto
{
    public GenderEnum Gender { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Name { get; set; } = null!;
    
}