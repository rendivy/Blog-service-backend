using blog_backend.DAO.Model;
using blog_backend.Service.Repository;

namespace blog_backend.Service;

public class PostService
{
    private readonly IPostRepository _postRepository;
    private readonly IAccountRepository _accountRepository;

    public PostService(IPostRepository postRepository, IAccountRepository accountRepository)
    {
        _postRepository = postRepository;
        _accountRepository = accountRepository;
    }
    
    public async Task<PostDTO?> GetPostDetails(Guid postId, Guid userId)
    {
        var post = await _postRepository.GetPostDetails(postId);
        if (post == null)
        {
            return null;
        }

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
            HasLike = post.LikedUsers.Any(u => u.Id == userId),
            Tags = post.Tags.Select(t => new TagDTO
            {
                Id = t.Id,
                Name = t.Name
            }).ToList()
        };
        return postDto;
    }


    public async Task UnlikePost(Guid userId, Guid postId)
    {
        var post = await _postRepository.GetPostDetails(postId);
        var user = await _accountRepository.GetUserById(userId.ToString());
        var isUserLiked = post?.LikedUsers?.Any(u => u.Id == userId);
        if (isUserLiked == null || !isUserLiked.Value)
        {
            throw new Exception("User not liked this post");
        }
        if (post == null || user == null)
        { 
            throw new Exception("Post or user not found");
        }
        await _postRepository.UnlikePost(post, user);
    }
}