using System.ComponentModel.DataAnnotations.Schema;

namespace blog_backend.DAO.Model;

public class PostDetailsDTO
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int ReadingTime { get; set; }
    public string Image { get; set; }
    public Guid AuthorId { get; set; }
    public string Author { get; set; }
    public string? AddressId { get; set; } = null;
    public Guid? CommunityId { get; set; } = null;
    public string? CommunityName { get; set; } = null;
    public int Likes { get; set; } = 0;
    public bool HasLike { get; set; }
    
    public List<CommentDTO>? Comments { get; set; } = new();
    public int CommentsCount { get; set; }
    
    [Column(TypeName = "uuid[]")]
    public List<TagDTO> Tags { get; set; } = new();
}