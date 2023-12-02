using blog_backend.DAO.Model;
using blog_backend.Service.Mappers;
using blog_backend.Service.Repository;

namespace blog_backend.Service;

public class PostService
{
    private readonly IPostRepository _postRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICommunityRepository _communityRepository;

    public PostService(IPostRepository postRepository, IAccountRepository accountRepository, ICommunityRepository communityRepository)
    {
        _postRepository = postRepository;
        _accountRepository = accountRepository;
        _communityRepository = communityRepository;
    }

    public async Task<PostDTO?> GetPostDetails(Guid postId, Guid userId)
    {
        var post = await _postRepository.GetPostDetails(postId);
        if (post == null)
        {
            return null;
        }
        var community = await _communityRepository.GetCommunityById(post.CommunityId);
        var comment = post.Comments.Where(c => c.CommentParent == null).Select(CommentMapper.Map).ToList();

        var postDto = new PostDTO
        {
            Id = post.Id,
            CreateTime = post.CreateTime,
            Title = post.Title,
            Description = post.Description,
            ReadingTime = post.ReadingTime,
            Image = post.Image,
            AuthorId = post.AuthorId,
            CommunityName = community?.Name,
            CommunityId = community?.Id,
            Author = post.Author,
            Comments = comment,
            Likes = post.LikedUsers.Count,
            HasLike = post.LikedUsers.Any(u => u.Id == userId),
            Tags = post.Tags.Select(t => new TagDTO
            {
                Id = t.Id,
                Name = t.Name,
                CreateTime = t.CreateTime
            }).ToList()
        };
        return postDto;
    }


    public async Task CreatePost(CreatePostDTO postDto, Guid userId)
    {
        var user = await _accountRepository.GetUserById(userId.ToString());
        var tags = await _postRepository.GetTags(postDto);
        var post = PostMapper.Map(postDto, tags, user.FullName, userId.ToString());
        if (user == null)
        {
            throw new Exception("User not found");
        }

        await _postRepository.CreatePost(post);
    }


    public async Task LikePost(Guid userId, Guid postId)
    {
        var post = await _postRepository.GetPostDetails(postId);
        var user = await _accountRepository.GetUserById(userId.ToString());
        var isUserLiked = post?.LikedUsers?.Any(u => u.Id == userId);
        
        if (isUserLiked == null || isUserLiked.Value)
        {
            throw new Exception("User already liked this post");
        }

        if (post == null || user == null)
        {
            throw new Exception("Post or user not found");
        }

        await _postRepository.LikePost(post, user);
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