using blog_backend.DAO.Database;
using blog_backend.DAO.Model;
using blog_backend.Entity;
using blog_backend.Entity.AccountEntities;
using blog_backend.Entity.CommentEntity;
using blog_backend.Service.Extensions;
using blog_backend.Service.Repository;

namespace blog_backend.Service;

public class CommentService
{
    private readonly BlogDbContext _blogDbContext;
    private const string UserNotFoundErrorMessage = "User not found";

    public CommentService(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }


    public async Task<List<CommentDTO>> GetNestedComments(string commentId)
    {
        var comments = await Guid.Parse(commentId).GetSubComments(_blogDbContext);
        var commentList = (from comment in comments
            let subComments = comment.Id.GetSubComments(_blogDbContext)
            select new CommentDTO
            {
                Id = comment.Id,
                ModifiedDate = comment.ModifiedDate,
                DeleteDate = comment.DeletedTime,
                Content = comment.Content,
                CreateTime = comment.CreatedTime,
                Author = comment.Author,
                AuthorId = comment.AuthorId,
                SubComments = subComments.Result.Count
            }).ToList();
        return await Task.FromResult(commentList);
    }


    public async Task DeleteComment(string commentId, string userId)
    {
        try
        {
            var comment = await Guid.Parse(commentId).GetCommentById(_blogDbContext);
            if (comment.CommentParent != null)
            {
                await DeleteChildComment(commentId, userId);
            }
            else await DeleteRouteComment(commentId, userId);
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Unexpected error while deleting comment: {e.Message}");
        }
    }


    private async Task DeleteChildComment(string commentId, string userId)
    {
        try
        {
            var comment = await Guid.Parse(commentId).GetCommentById(_blogDbContext);
            var parentComment = await comment.CommentParent.Id.GetCommentById(_blogDbContext);
            CommentServiceExtension.ValidateCommentRequest(comment, userId);
            if (comment.SubComments > 0)
            {
                comment.DeletedTime = DateTime.Now;
                comment.Content = null;
            }
            else
            {
                _blogDbContext.Comments.Remove(comment);
            }

            parentComment.SubComments--;
            if (parentComment.SubComments == 0)
            {
                _blogDbContext.Comments.Remove(parentComment);
            }
            else
            {
                _blogDbContext.Comments.Update(parentComment);
            }
            await _blogDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Unexpected error while deleting comment: {e.Message}");
        }
    }

    private async Task DeleteRouteComment(string commentId, string userId)
    {
        try
        {
            var comment = await Guid.Parse(commentId).GetCommentById(_blogDbContext);
            CommentServiceExtension.ValidateCommentRequest(comment, userId);

            if (comment.SubComments > 0)
            {
                comment.DeletedTime = DateTime.Now;
                comment.Content = null;
            }
            else _blogDbContext.Comments.Remove(comment);

            await _blogDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Unexpected error while deleting comment: {e.Message}");
        }
    }


    public async Task EditComment(EditCommentDTO editCommentDto, string commentId, string userId)
    {
        try
        {
            var comment = await Guid.Parse(commentId).GetCommentById(_blogDbContext);
            CommentServiceExtension.ValidateCommentRequest(comment, userId);

            comment.Content = editCommentDto.Content;
            comment.ModifiedDate = DateTime.Now;

            _blogDbContext.Comments.Update(comment);
            await _blogDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Unexpected error while editing comment: {e.Message}");
        }
    }


    public async Task CreateComment(AddCommentDTO addCommentDto, string postId, string userId)
    {
        var user = await userId.GetUserById(_blogDbContext);
        if (user == null) throw new Exception(UserNotFoundErrorMessage);
        if (addCommentDto.ParentId == null || addCommentDto.ParentId == Guid.Empty)
        {
            await CreateRootComment(addCommentDto, postId, user);
        }
        else
        {
            await CreateChildComment(addCommentDto, postId, user);
        }
    }

    private async Task CreateRootComment(AddCommentDTO addCommentDto, string postId, User user)
    {
        try
        {
            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                PostId = Guid.Parse(postId),
                CreatedTime = DateTime.Now,
                ModifiedDate = null,
                DeletedTime = null,
                User = user,
                Content = addCommentDto.Content,
                Author = user.FullName,
                AuthorId = user.Id.ToString(),
                CommentParent = null
            };
            await _blogDbContext.Comments.AddAsync(comment);
            await _blogDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Unexpected error while creating comment: {e.Message}");
        }
    }

    private async Task CreateChildComment(AddCommentDTO addCommentDto, string postId, User user)
    {
        try
        {
            var parentComment = await addCommentDto.ParentId?.GetCommentById(_blogDbContext)!;
            if (parentComment != null)
            {
                parentComment.SubComments++;
                var comment = new Comment
                {
                    Id = Guid.NewGuid(),
                    PostId = Guid.Parse(postId),
                    CreatedTime = DateTime.Now,
                    ModifiedDate = null,
                    DeletedTime = null,
                    User = user,
                    Content = addCommentDto.Content,
                    Author = user.FullName,
                    AuthorId = user.Id.ToString(),
                    CommentParent = parentComment
                };
                await _blogDbContext.Comments.AddAsync(comment);
                await _blogDbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Parent comment not found");
            }
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Unexpected error while creating comment: {e.Message}");
        }
    }
}