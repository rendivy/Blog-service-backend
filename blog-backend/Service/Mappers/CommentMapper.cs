using blog_backend.DAO.Model;
using blog_backend.Entity;

namespace blog_backend.Service.Mappers;

public class CommentMapper
{
    public static CommentDTO Map(Comment comment)
    {
        return new CommentDTO
        {
            Id = comment.Id,
            AuthorId = comment.AuthorId,
            Author = comment.Author,
            Content = comment.Content,
            CreateTime = comment.CreatedTime,
            SubComments = comment.SubComments,
            ModifiedDate = comment.ModifiedDate,
            DeleteDate = comment.DeletedTime,
        };
    }
}