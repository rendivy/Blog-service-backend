using System.ComponentModel.DataAnnotations.Schema;

namespace blog_backend.DAO.Model;

public class PostDTO
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int ReadingTime { get; set; }
    public string Image { get; set; }
    public Guid AuthorId { get; set; }
    public string Author { get; set; }
    public Guid? CommunityId { get; set; } = null;
    public string? CommunityName { get; set; } = null;
    public int Likes { get; set; } = 0;
    public bool HasLike { get; set; }
    
    [Column(TypeName = "uuid[]")]
    public List<TagDTO> Tags { get; set; } = new();
}