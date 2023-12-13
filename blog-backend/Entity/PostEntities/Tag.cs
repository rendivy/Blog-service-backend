using System.ComponentModel.DataAnnotations;

namespace blog_backend.Entity.PostEntities;

public class Tag
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public DateTime CreateTime { get; set; } = DateTime.Now;
    public List<Post>? Posts { get; set; } = new();
}