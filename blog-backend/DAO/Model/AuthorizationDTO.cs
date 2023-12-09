using blog_backend.DAO.Utils;

namespace blog_backend.DAO.Model;

public class AuthorizationDTO
{
    public  GenderEnum Gender { get; set; }
    public required string Email { get; set; } = null!;
    public required string Password { get; set; } = null!;
    public DateTime DateOfBirth { get; set; } = DateTime.Now;
    public required string FullName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
}