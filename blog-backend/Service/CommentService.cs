using blog_backend.DAO.Model;
using blog_backend.Entity;
using blog_backend.Entity.AccountEntities;
using blog_backend.Entity.CommentEntity;
using blog_backend.Service.Repository;

namespace blog_backend.Service;

public class CommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IAccountRepository _accountRepository;
    private const string CommentNotFoundErrorMessage = "Comment not found";
    private const string NotAuthorOfCommentErrorMessage = "You are not the author of this comment";
    private const string CommentAlreadyDeletedErrorMessage = "Comment already deleted";
    private const string UserNotFoundErrorMessage = "User not found";

    public CommentService(ICommentRepository commentRepository, IAccountRepository accountRepository)
    {
        _commentRepository = commentRepository;
        _accountRepository = accountRepository;
    }


    public async Task<List<CommentDTO>> GetNestedComments(string commentId)
    {
        var comments = _commentRepository.GetSubComments(Guid.Parse(commentId)).Result;
        var commentList = (from comment in comments
            let subComments = _commentRepository.GetSubComments(comment.Id).Result
            select new CommentDTO
            {
                Id = comment.Id,
                ModifiedDate = comment.ModifiedDate,
                DeleteDate = comment.DeletedTime,
                Content = comment.Content,
                CreateTime = comment.CreatedTime,
                Author = comment.Author,
                AuthorId = comment.AuthorId,
                SubComments = subComments.Count
            }).ToList();
        return await Task.FromResult(commentList);
    }


    public async Task DeleteComment(string commentId, string userId)
    {
        var comment = _commentRepository.GetCommentById(new Guid(commentId)).Result;
        if (comment == null) throw new Exception(CommentNotFoundErrorMessage);
        if (userId != comment.AuthorId) throw new Exception(NotAuthorOfCommentErrorMessage);
        comment.DeletedTime = DateTime.Now;
        if (comment.Content == null) throw new Exception(CommentAlreadyDeletedErrorMessage);
        if (comment.SubComments > 0)
        {
            comment.Content = null;
            await _commentRepository.SaveChangesAsync();
        }
        else
        {
            var status = _commentRepository.DeleteComment(comment);
            if (status.IsCompletedSuccessfully)
            {
                await _commentRepository.SaveChangesAsync();
            }
        }
    }


    public async Task EditComment(EditCommentDTO editCommentDto, string commentId, string userId)
    {
        var comment = _commentRepository.GetCommentById(new Guid(commentId)).Result;
        if (comment == null) throw new ArgumentException(CommentNotFoundErrorMessage);
        if (userId != comment.AuthorId) throw new ArgumentException(NotAuthorOfCommentErrorMessage);
        comment.Content = editCommentDto.Content;
        comment.ModifiedDate = DateTime.Now;
        var status = _commentRepository.EditComment(comment);
        if (status.IsCompletedSuccessfully)
        {
            await _commentRepository.SaveChangesAsync();
        }
    }


    public async Task CreateComment(AddCommentDTO addCommentDto, string postId, string userId)
    {
        var user = await _accountRepository.GetUserById(userId);
        if (user == null) throw new Exception( UserNotFoundErrorMessage);
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

        var status = _commentRepository.CreateComment(comment);
        if (status.IsCompletedSuccessfully)
        {
            await _commentRepository.SaveChangesAsync();
        }
    }

    private async Task CreateChildComment(AddCommentDTO addCommentDto, string postId, User user)
    {
        var parentComment = await _commentRepository.GetCommentById(addCommentDto.ParentId);

        if (parentComment != null)
        {
            parentComment.SubComments++;
        }

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

        var status = _commentRepository.CreateComment(comment);

        if (status.IsCompletedSuccessfully)
        {
            await _commentRepository.SaveChangesAsync();
        }
    }


    public Task<List<CommentDTO>> GetCommentsByPostId(Guid postId)
    {
        var comments = _commentRepository.GetCommentsByPostId(postId).Result;
        var commentList = (from comment in comments
            let subComments = _commentRepository.GetSubComments(comment.Id).Result
            select new CommentDTO
            {
                Id = comment.Id,
                ModifiedDate = comment.ModifiedDate,
                DeleteDate = comment.DeletedTime,
                Content = comment.Content,
                CreateTime = comment.CreatedTime,
                Author = comment.Author,
                AuthorId = comment.AuthorId,
                SubComments = subComments.Count
            }).ToList();
        return Task.FromResult(commentList);
    }
}