using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using blog_backend.Service;

namespace blog_backend.Entity;

public class Post
{
    [Key] 
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; } = DateTime.Now;
    public string Title { get; set; }
    public string Description { get; set; }
    public int ReadingTime { get; set; }
    public string Image { get; set; }
    public Guid AuthorId { get; set; }
    [Required]
    public string Author { get; set; }
    public int Likes { get; set; } 
    public List<User>? LikedUsers { get; set; } = new();
    public List<Tag>? Tags { get; set; } = new();
}

