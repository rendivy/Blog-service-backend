using System.ComponentModel.DataAnnotations;

namespace blog_backend.Entity;

public class Comment
{
    [Key] 
    public Guid Id { get; set; }
    public DateTime? ModifiedDate { get; set; } = null;
    public DateTime CreatedTime { get; set; }
    public DateTime? DeletedTime { get; set; } = null;
    public string Author { get; set; }
    public string AuthorId { get; set; }
    public User User { get; set; } = null!;
    public string? Content { get; set; } = string.Empty;
    public int SubComments { get; set; } = 0;
    public Guid PostId { get; set; }
    public Comment? CommentParent { get; set; } = null!;
    public Post Post { get; set; } = null!;
}