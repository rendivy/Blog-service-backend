namespace blog_backend.DAO.Model;

public class LoginDTO
{
    public required string Email { get; set; } = null!;
    public required string Password { get; set; } = null!;
}   