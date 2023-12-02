using blog_backend.DAO.Model;
using blog_backend.Entity;
using blog_backend.Service.Repository;

namespace blog_backend.Service;

public class CommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IAccountRepository _accountRepository;

    public CommentService(ICommentRepository commentRepository, IAccountRepository accountRepository)
    {
        _commentRepository = commentRepository;
        _accountRepository = accountRepository;
    }


    public async Task CreateComment(AddCommentDTO addCommentDto, string postId, string userId)
    {
        var userFullName = _accountRepository.GetUserById(userId).Result?.FullName;
            
        var comment = new Comment
        {
            Id = Guid.NewGuid(),
            PostId = Guid.Parse(postId),
            CreatedTime = DateTime.Now,
            ModifiedDate = null,
            DeletedTime = null,
            Content = addCommentDto.Content,
            Author = userFullName,
            AuthorId = userId,
            CommentParent = null
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