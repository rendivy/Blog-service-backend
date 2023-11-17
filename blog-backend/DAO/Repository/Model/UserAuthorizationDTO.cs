using blog_backend.DAO.Repository.Utils;

namespace blog_backend.Dao.Repository.Model;

public class UserAuthorizationDTO
{
    public GenderEnum Gender { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}