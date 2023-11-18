using blog_backend.DAO.Repository.Utils;

namespace blog_backend.Dao.Repository.Model;

public class UserAuthorizationDto
{
    public  GenderEnum Gender { get; set; }
    public required string Email { get; set; } = null!;
    public required string Password { get; set; } = null!;
    public required string Name { get; set; } = null!;
    
}