using blog_backend.DAO.Model;
using blog_backend.Entity;
using blog_backend.Entity.PostEntities;

namespace blog_backend.Service.Mappers;

public static class PostMapper
{
    public static PostDetailsDTO MapDetails(Post post)
    {
        var postDto = new PostDetailsDTO
        {
            Id = post.Id,
            CreateTime = post.CreateTime,
            Title = post.Title,
            Description = post.Description,
            ReadingTime = post.ReadingTime,
            Image = post.Image,
            Comments = post.Comments.Select(CommentMapper.Map).ToList(),
            CommentsCount = post.Comments.Count,
            AuthorId = post.AuthorId,
            Author = post.Author,
            Likes = post.LikedUsers.Count,
            Tags = post.Tags.Select(t => new TagDTO
            {
                Id = t.Id,
                Name = t.Name,
                CreateTime = t.CreateTime
            }).ToList()
        };
        return postDto;
    }
    
    public static PostDTO MapToDTO(List<PostDetailsDTO> posts, PaginationDTO paginationDTO)
    {
        return new PostDTO
        {
            Posts = posts,
            Pagination = paginationDTO
        };
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