namespace blog_backend.DAO.Model;

public class CommentDTO
{
    public string Content { get; set; } = string.Empty;
    public DateTime? ModifiedDate { get; set; } = null;
    public DateTime? DeleteDate { get; set; } = null;
    public string AuthorId { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int SubComments { get; set; } = 0;
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    
}
