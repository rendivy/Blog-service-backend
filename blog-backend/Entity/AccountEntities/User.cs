using System.ComponentModel.DataAnnotations;
using blog_backend.Entity.CommunityEntities;
using blog_backend.Entity.PostEntities;
using blog_backend.Enums;
using Microsoft.EntityFrameworkCore;

namespace blog_backend.Entity.AccountEntities;

public class User
{
    [Key] public Guid Id { get; set; }

    [Required]
    [MaxLength(255)]
    [RegularExpression("^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$", ErrorMessage = "Invalid Email pattern")]
    public string Email { get; set; } = string.Empty;

    public DateTime CreateTime { get; set; } = DateTime.Now;
    [Required] [MinLength(6)] public string Password { get; set; } = string.Empty;
    [Required] [MinLength(2)] public string FullName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; } = DateTime.Now;

    [RegularExpression("^\\+7\\d{10}$", ErrorMessage = "Invalid Phone pattern")]
    public string PhoneNumber { get; set; } = null!;

    public GenderEnum Gender { get; set; }
    public List<Post>? LikedPosts { get; set; } = new();
    public List<CommunityMembership>? CommunityMemberships { get; set; } = new();
}