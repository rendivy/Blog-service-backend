using blog_backend.DAO.Model;
using blog_backend.Entity;

namespace blog_backend.Service.Mappers;

public static class PostMapper
{
    public static PostDTO MapDetails(Post post)
    {
        var postDto = new PostDTO
        {
            Id = post.Id,
            CreateTime = post.CreateTime,
            Title = post.Title,
            Description = post.Description,
            ReadingTime = post.ReadingTime,
            Image = post.Image,
            AuthorId = post.AuthorId,
            Author = post.Author,
            Likes = post.LikedUsers.Count,
            Tags = post.Tags.Select(t => new TagDTO
            {
                Id = t.Id,
                Name = t.Name
            }).ToList()
        };
        return postDto;
    }
    
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