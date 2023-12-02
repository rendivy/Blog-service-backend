using blog_backend.DAO.Database;
using blog_backend.Entity;
using blog_backend.Service.Repository;

namespace blog_backend.DAO.Repository;

public class CommentRepository : ICommentRepository
{
    private readonly BlogDbContext _blogDbContext;

    public CommentRepository(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }

    public Task CreateComment(Comment comment)
    {
        _blogDbContext.Comments.AddAsync(comment);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync()
    {
        return _blogDbContext.SaveChangesAsync();
    }


    public Task<Comment?> GetCommentById(Guid? id)
    {
        return Task.FromResult(_blogDbContext.Comments.FirstOrDefault(c => c.Id == id));
    }

    public Task<List<Comment>> GetCommentsByPostId(Guid postId)
    {
        return Task.FromResult(_blogDbContext.Comments.Where(c => c.PostId == postId && c.CommentParent == null)
            .ToList());
    }

    public Task<List<Comment>> GetSubComments(Guid commentId)
    {
        return Task.FromResult(_blogDbContext.Comments
            .Where(c => c.CommentParent != null && c.CommentParent.Id == commentId).ToList());
    }
}