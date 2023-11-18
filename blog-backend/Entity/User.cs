using System.ComponentModel.DataAnnotations;
using blog_backend.DAO.Repository.Utils;

namespace blog_backend.Entity;

public class User
{
    [Key]
    public Guid Id { get; set; }  
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; } = DateTime.Now;
    public string PhoneNumber { get; set; } = null!;
    public GenderEnum Gender { get; set; }
}