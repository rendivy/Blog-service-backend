namespace blog_backend.DAO.Model.Comment;

public class AddCommentDTO
{
    public string Content { get; set; } = string.Empty;
    public Guid? ParentId { get; set; } = null;
}