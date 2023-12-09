using System.ComponentModel.DataAnnotations;

namespace blog_backend.Entity;

public class Community
{
    [Key] 
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; } = DateTime.Now;
    [MinLength(5)] [MaxLength(50)] 
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsClosed { get; set; }
    public int SubscribersCount { get; set; } = 0;
    public List<Post>? Posts { get; set; } = new();
    public List<CommunityMembership>? Memberships { get; set; } = new();
}