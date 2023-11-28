using blog_backend.DAO.Model;
using blog_backend.Entity;

namespace blog_backend.Service.Mappers;

public class PostMapper
{
    public static Post Map(CreatePostDTO postDto, List<Tag> tags, string userName, string userId)
    {
        var post = new Post
        {
            Title = postDto.Title,
            Description = postDto.Description,
            ReadingTime = postDto.ReadingTime,
            Image = postDto.Image,
            Author = userName,
            AuthorId = new Guid(userId),
            Tags = tags
        };
        return post;
    }
}