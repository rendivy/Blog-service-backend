using blog_backend.Entity;
using blog_backend.Entity.CommentEntity;

namespace blog_backend.Service.Repository;

public interface ICommentRepository
{
    public  Task CreateComment(Comment comment);
    
    public Task SaveChangesAsync();
    public Task DeleteComment(Comment comment);
    public Task EditComment(Comment comment);
    public Task<Comment?> GetCommentById(Guid? id);
    public Task<List<Comment>> GetCommentsByPostId(Guid postId);
    public Task<List<Comment>> GetSubComments(Guid commentId);
}