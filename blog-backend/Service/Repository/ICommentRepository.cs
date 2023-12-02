using blog_backend.Entity;

namespace blog_backend.Service.Repository;

public interface ICommentRepository
{
    public  Task CreateComment(Comment comment);
    
    public Task SaveChangesAsync();
    public Task<Comment?> GetCommentById(Guid id);
    public Task<List<Comment>> GetCommentsByPostId(Guid postId);
    public Task<List<Comment>> GetSubComments(Guid commentId);
}