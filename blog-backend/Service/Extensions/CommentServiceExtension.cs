using blog_backend.DAO.Database;
using blog_backend.Entity.CommentEntity;
using Microsoft.EntityFrameworkCore;

namespace blog_backend.Service.Extensions;

public static class CommentServiceExtension
{
    private const string CommentNotFoundErrorMessage = "Comment not found";
    private const string NotAuthorOfCommentErrorMessage = "You are not the author of this comment";
    private const string CommentAlreadyDeletedErrorMessage = "Comment already deleted";

    public static async Task<Comment?> GetCommentById(this Guid id, BlogDbContext dbContext)
    {
        return await dbContext.Comments.Include(c => c.CommentParent)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public static void ValidateCommentRequest(Comment? comment, string userId)
    {
        if (comment == null) throw new Exception(CommentNotFoundErrorMessage);
        if (userId != comment.AuthorId) throw new Exception(NotAuthorOfCommentErrorMessage);
        if (comment.Content == null) throw new Exception(CommentAlreadyDeletedErrorMessage);
    }

    public static async Task<List<Comment>> GetSubComments(this Guid commentId, BlogDbContext dbContext)
    {
        return await dbContext.Comments
            .Where(c => c.CommentParent != null && c.CommentParent.Id == commentId)
            .ToListAsync();
    }


    public static async Task<List<Comment>> GetCommentsByPostId(this Guid postId, BlogDbContext dbContext)
    {
        return await dbContext.Comments
            .Where(c => c.PostId == postId)
            .ToListAsync();
    }
}