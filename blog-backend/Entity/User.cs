using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using blog_backend.DAO.Utils;

namespace blog_backend.Entity;

public class User
{
    [Key] 
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime CreateTime { get; set; } = DateTime.Now;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; } = DateTime.Now;
    public string PhoneNumber { get; set; } = null!;
    public GenderEnum Gender { get; set; }
    public List<Post>? LikedPosts { get; set; } = new ();
    public List<CommunityMembership>? CommunityMemberships { get; set; } = new();
}