using System.ComponentModel.DataAnnotations;
using blog_backend.DAO.Repository.Utils;

namespace blog_backend.Entity;

public class User
{
    [Key]
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Name { get; set; } = null!;
    public GenderEnum Gender { get; set; }
}