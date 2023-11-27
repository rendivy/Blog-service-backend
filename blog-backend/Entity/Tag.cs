namespace blog_backend.Entity;

public class Tag
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public DateTime CreateTime { get; set; } = DateTime.Now;
    public List<Post> Posts { get; set; } = new();
    public List<PostTag> PostTags { get; } = new();
}